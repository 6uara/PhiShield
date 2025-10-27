extends Control

# --- CONSTANTES ---
# Ruta para guardar la configuración
const SAVE_PATH = "user://settings.cfg"

# Nombres de los buses de audio (asegúrate de que existan en tu proyecto)
const MASTER_BUS_NAME = "Master"
const MUSIC_BUS_NAME = "Music"
const SFX_BUS_NAME = "SFX"

# --- REFERENCIAS A NODOS ---
@onready var master_slider: HSlider = %MasterSlider
@onready var music_slider: HSlider = %MusicSlider
@onready var sfx_slider: HSlider = %SfxSlider
@onready var resolution_button: OptionButton = %ResolutionButton
@onready var fullscreen_toggle: CheckBox = %FullscreenToggle
@onready var vsync_toggle: CheckBox = %VSyncToggle
@onready var back_button: Button = %BackButton
@onready var back_button_2: Button = %BackButton2

# Variables para guardar los índices de los buses
var _master_bus: int
var _music_bus: int
var _sfx_bus: int

# Lista de resoluciones (puedes añadir o quitar las que quieras)
var _resolutions = [
	Vector2i(1280, 720),
	Vector2i(1600, 900),
	Vector2i(1920, 1080),
	Vector2i(2560, 1440)
]


func _ready() -> void:
	_master_bus = AudioServer.get_bus_index(MASTER_BUS_NAME)
	_music_bus = AudioServer.get_bus_index(MUSIC_BUS_NAME)
	_sfx_bus = AudioServer.get_bus_index(SFX_BUS_NAME)

	for res in _resolutions:
		resolution_button.add_item("%d x %d" % [res.x, res.y])

	master_slider.value_changed.connect(_on_master_volume_changed)
	music_slider.value_changed.connect(_on_music_volume_changed)
	sfx_slider.value_changed.connect(_on_sfx_volume_changed)
	resolution_button.item_selected.connect(_on_resolution_selected)
	fullscreen_toggle.toggled.connect(_on_fullscreen_toggled)
	vsync_toggle.toggled.connect(_on_vsync_toggled)
	back_button_2.pressed.connect(_on_back_pressed)
	back_button.pressed.connect(_on_back_pressed)
	
	load_options()


# --- FUNCIONES DE AUDIO ---

func _on_master_volume_changed(value: float) -> void:
	_set_bus_volume(_master_bus, value)

func _on_music_volume_changed(value: float) -> void:
	_set_bus_volume(_music_bus, value)

func _on_sfx_volume_changed(value: float) -> void:
	_set_bus_volume(_sfx_bus, value)

func _set_bus_volume(bus_index: int, linear_value: float) -> void:
	AudioServer.set_bus_volume_db(bus_index, linear_to_db(linear_value))


# --- FUNCIONES DE VIDEO ---

func _on_resolution_selected(index: int) -> void:
	var selected_res = _resolutions[index]
	DisplayServer.window_set_size(selected_res)

func _on_fullscreen_toggled(is_pressed: bool) -> void:
	if is_pressed:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	else:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)

func _on_vsync_toggled(is_pressed: bool) -> void:
	if is_pressed:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_ENABLED)
	else:
		DisplayServer.window_set_vsync_mode(DisplayServer.VSYNC_DISABLED)


# --- GUARDAR Y CARGAR ---

func _on_back_pressed() -> void:
	save_options()
	hide() 

func save_options() -> void:
	var config = ConfigFile.new()
	
	config.set_value("audio", "master_volume", master_slider.value)
	config.set_value("audio", "music_volume", music_slider.value)
	config.set_value("audio", "sfx_volume", sfx_slider.value)
	
	config.set_value("video", "resolution_index", resolution_button.selected)
	config.set_value("video", "fullscreen", fullscreen_toggle.button_pressed)
	config.set_value("video", "vsync", vsync_toggle.button_pressed)
	
	var error = config.save(SAVE_PATH)
	if error != OK:
		printerr("Error guardando configuración: ", error)

func load_options() -> void:
	var config = ConfigFile.new()
	
	var error = config.load(SAVE_PATH)
	if error != OK:
		_apply_loaded_options()
		return

	# --- Cargar y Asignar Valores ---
	# Usamos get_value con un valor por defecto por si la clave no existe
	
	# Audio
	master_slider.value = config.get_value("audio", "master_volume", 1.0)
	music_slider.value = config.get_value("audio", "music_volume", 1.0)
	sfx_slider.value = config.get_value("audio", "sfx_volume", 1.0)
	
	# Video
	resolution_button.select(config.get_value("video", "resolution_index", 2)) # Default: 1920x1080 (índice 2)
	fullscreen_toggle.button_pressed = config.get_value("video", "fullscreen", false)
	vsync_toggle.button_pressed = config.get_value("video", "vsync", true) # VSync suele ser bueno por defecto

	# Aplicamos los valores cargados
	_apply_loaded_options()


# Función para aplicar todos los settings de una vez al cargar
func _apply_loaded_options() -> void:
	_on_master_volume_changed(master_slider.value)
	_on_music_volume_changed(music_slider.value)
	_on_sfx_volume_changed(sfx_slider.value)
	_on_resolution_selected(resolution_button.selected)
	_on_fullscreen_toggled(fullscreen_toggle.button_pressed)
	_on_vsync_toggled(vsync_toggle.button_pressed)
