extends Control

# --- CONSTANTE DE GUARDADO ---
# Godot guardará la foto en la carpeta de datos del usuario
const PROFILE_PIC_PATH = "user://profile_pic.png"

# --- NODOS DE LA ESCENA ---
@export var profile_pic: TextureRect
@export var change_button: Button 
@export var file_dialog: FileDialog 

func _ready() -> void:
	# 1. Conectar las señales
	change_button.pressed.connect(_on_change_button_pressed)
	file_dialog.file_selected.connect(_on_file_selected)
	
	# 2. Intentar cargar la foto guardada al iniciar
	load_profile_pic()

# --- FUNCIONES ---

## 1. Muestra el popup para elegir archivo
func _on_change_button_pressed() -> void:
	file_dialog.popup_centered()

## 2. Se llama cuando el usuario elige un archivo
func _on_file_selected(path: String) -> void:
	# Cargar el archivo seleccionado como una 'Image'
	var image = Image.load_from_file(path)
	
	# Comprobar si la imagen se cargó correctamente
	if image.is_empty():
		printerr("Error: No se pudo cargar la imagen desde ", path)
		return
		
	# ¡Éxito! Guardamos la imagen para futuras sesiones
	_save_profile_pic(image)
	
	# Mostramos la imagen en el TextureRect
	_display_image(image)


## 3. Convierte una 'Image' en una 'Texture' para mostrarla
func _display_image(image: Image) -> void:
	# Los nodos 'TextureRect' no usan 'Image', usan 'Texture'.
	# Así que debemos convertirla:
	var texture = ImageTexture.create_from_image(image)
	
	# Asignamos la nueva textura
	profile_pic.texture = texture


## 4. Guarda la imagen en la carpeta 'user://'
func _save_profile_pic(image: Image) -> void:
	# Guardamos una copia de la imagen como PNG en nuestra carpeta de datos
	var err = image.save_png(PROFILE_PIC_PATH)
	if err != OK:
		printerr("Error: No se pudo guardar la foto de perfil: ", err)


## 5. Carga la imagen guardada (se llama en _ready)
func load_profile_pic() -> void:
	# Comprobar si ya existe una foto guardada
	if not FileAccess.file_exists(PROFILE_PIC_PATH):
		print("No se encontró foto de perfil guardada.")
		return
		
	# Cargar la imagen guardada
	var image = Image.load_from_file(PROFILE_PIC_PATH)
	
	if not image.is_empty():
		print("Foto de perfil cargada.")
		_display_image(image)
