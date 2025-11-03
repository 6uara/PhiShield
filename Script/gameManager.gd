# GameManager.gd
extends Node

## -----------------------------------------------------------------
## SEÑALES (Signals)
## -----------------------------------------------------------------
# Se emite cada vez que el jugador hace un intento (para actualizar la UI)
signal trial_updated(successes, failures, total_attempts)
# Se emite cuando se completan los 5 intentos
signal trial_finished(module_completed, unlocked_next_module, current_module)
# Se emite cuando los datos del jugador (nombre, etc.) cambian
signal player_data_changed(player_name, completed_modules)


## -----------------------------------------------------------------
## DATOS PERSISTENTES (Se guardan)
## -----------------------------------------------------------------
var player_name: String = "Analista"
# Un array de enums que guarda los módulos completados
var completed_modules: Array[difficultyEnums.difficulty] = []

const SAVE_PATH = "user://game_data.save" # Ruta de guardado

## -----------------------------------------------------------------
## DATOS DE PRUEBA (Temporales)
## -----------------------------------------------------------------
var current_module: difficultyEnums.difficulty
var current_successes: int = 0
var current_failures: int = 0
var current_attempt_count: int = 0

const MAX_ATTEMPTS: int = 5
const MIN_SUCCESS_TO_PASS: int = 3

var _current_desktop : Control
var _current_tutorial : Control

func _ready() -> void:
	load_game()


# -----------------------------------------------------------------
# API PÚBLICA: Iniciar y Registrar Pruebas
# -----------------------------------------------------------------

## Llama a esto desde Desktop.gd cuando empieza un módulo (en _ready)
func start_module_trial(module: difficultyEnums.difficulty) -> void:
	current_module = module
	current_successes = 0
	current_failures = 0
	current_attempt_count = 0
	emit_signal("trial_updated", 0, 0, 0)


## Llama a esto desde Desktop.gd cuando el jugador toma una decisión
func record_attempt(was_successful: bool) -> void:
	if current_attempt_count >= MAX_ATTEMPTS: return

	current_attempt_count += 1
	if was_successful:
		current_successes += 1
	else:
		current_failures += 1
	
	emit_signal("trial_updated", current_successes, current_failures, current_attempt_count)

	if current_attempt_count == MAX_ATTEMPTS:
		_finish_trial()


# -----------------------------------------------------------------
# API PÚBLICA: Datos del Jugador y Módulos
# -----------------------------------------------------------------

func set_player_name(new_name: String) -> void:
	player_name = new_name
	save_game()
	emit_signal("player_data_changed", player_name, completed_modules)

func get_player_name() -> String:
	return player_name

func is_module_completed(module: difficultyEnums.difficulty) -> bool:
	return completed_modules.has(module)

func is_module_unlocked(module: difficultyEnums.difficulty) -> bool:
	if module == difficultyEnums.difficulty.Bronce:
		return true
	if module == difficultyEnums.difficulty.Plata:
		return is_module_completed(difficultyEnums.difficulty.Bronce)
	if module == difficultyEnums.difficulty.Oro:
		return is_module_completed(difficultyEnums.difficulty.Plata)
	return false # Por si se añaden más

func get_next_module(module: difficultyEnums.difficulty) -> Variant:
	match module:
		difficultyEnums.difficulty.Bronce:
			return difficultyEnums.difficulty.Plata
		difficultyEnums.difficulty.Plata:
			return difficultyEnums.difficulty.Oro
		difficultyEnums.difficulty.Oro:
			return null # No hay más módulos
	return null

# -----------------------------------------------------------------
# LÓGICA INTERNA
# -----------------------------------------------------------------

## Se llama automáticamente al llegar a 5 intentos
func _finish_trial() -> void:
	var passed = (current_successes >= MIN_SUCCESS_TO_PASS)
	var next_module = null
	
	if passed:
		if not completed_modules.has(current_module):
			completed_modules.append(current_module)
		next_module = get_next_module(current_module)
		save_game()
		
	emit_signal("trial_finished", passed, next_module,current_module)
	if Engine.is_editor_hint():
		print("Se esta jugando desde el editor")
		return
	else:
		var params = {
		"module_name": difficultyEnums.difficulty.keys()[current_module],
		"passed_module": passed,
		"correct_answers": current_successes,
		"incorrect_answers": current_failures
		}
		print("Info Enviada")
		AnalyticsManager.send_event("trial_finished", params)


func _finish_tutorial() -> void:
	_current_tutorial.visible = false
	_current_desktop.visible = true

# -----------------------------------------------------------------
# SISTEMA DE GUARDADO 
# -----------------------------------------------------------------

func save_game() -> void:
	var config = ConfigFile.new()
	
	config.set_value("player", "name", player_name)
	
	# ConfigFile no guarda arrays de enums, así que lo convertimos a un Array de Ints
	var modules_as_ints: Array[int] = []
	for module_enum in completed_modules:
		modules_as_ints.append(module_enum)
		
	config.set_value("player", "completed_modules", modules_as_ints)
	
	var err = config.save(SAVE_PATH)
	if err != OK:
		printerr("GameManager: Error al guardar datos: ", err)
	else:
		print("GameManager: Progreso guardado.")

func load_game() -> void:
	var config = ConfigFile.new()
	var err = config.load(SAVE_PATH)
	
	if err != OK:
		if err == ERR_FILE_NOT_FOUND:
			print("GameManager: No se encontró archivo de guardado. Creando uno nuevo.")
			save_game() # Guardar por primera vez
		else:
			printerr("GameManager: Error al cargar datos: ", err)
		return

	# Cargar datos
	player_name = config.get_value("player", "name", "Analista")
	
	# Convertir los Ints guardados de nuevo a Enums
	var modules_as_ints = config.get_value("player", "completed_modules", [])
	completed_modules.clear()
	for module_int in modules_as_ints:
		completed_modules.append(module_int as difficultyEnums.difficulty)
	
	print("GameManager: Datos cargados. Jugador: %s" % player_name)
	emit_signal("player_data_changed", player_name, completed_modules)
