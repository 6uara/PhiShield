extends Control

# --- CONSTANTE DE GUARDADO ---
# Godot guardará la foto en la carpeta de datos del usuario
const PROFILE_PIC_PATH = "user://profile_pic.png"

# --- NODOS DE LA ESCENA ---
@export var profile_pic: TextureRect
@export var change_button: Button 
@export var file_dialog: FileDialog 
@export var namechange : Button
@export var alias : LineEdit
@export var edad: LineEdit 
@export var puesto: LineEdit 
@export var confirm : Button


func _ready() -> void:
	change_button.pressed.connect(_on_change_button_pressed)
	file_dialog.file_selected.connect(_on_file_selected)
	if(GameManager.get_player_name() == null):
		alias.text = "Analista"
	else:
		alias.text = GameManager.get_player_name()
	load_profile_pic()

# --- FUNCIONES ---

func _on_change_button_pressed() -> void:
	file_dialog.popup_centered()

func _on_file_selected(path: String) -> void:
	var image = Image.load_from_file(path)
	if image.is_empty():
		printerr("Error: No se pudo cargar la imagen desde ", path)
		return
	_save_profile_pic(image)
	_display_image(image)


func _display_image(image: Image) -> void:
	var texture = ImageTexture.create_from_image(image)
	profile_pic.texture = texture

func _save_profile_pic(image: Image) -> void:
	var err = image.save_png(PROFILE_PIC_PATH)
	if err != OK:
		printerr("Error: No se pudo guardar la foto de perfil: ", err)

func load_profile_pic() -> void:
	if not FileAccess.file_exists(PROFILE_PIC_PATH):
		print("No se encontró foto de perfil guardada.")
		return
	var image = Image.load_from_file(PROFILE_PIC_PATH)
	if not image.is_empty():
		print("Foto de perfil cargada.")
		_display_image(image)


func _on_cambiar_nombre_pressed() -> void:
	alias.editable = true
	edad.editable = true
	puesto.editable = true
	confirm.show()


func _on_confirm_change_pressed() -> void:
	GameManager.save_all_data(int(edad.text),puesto.text,alias.text)
	alias.editable = false
	edad.editable = false
	puesto.editable = false
	confirm.hide()
	
