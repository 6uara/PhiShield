extends Control
@onready var select_dificulty: OptionButton = $PlayContainer/SelectDificulty
@onready var timer: Timer = $Timer
@onready var options_menu: Control = $OptionsMenu

func _ready() -> void:
	AudioManager.play_music()
	var moduloscompletados = GameManager.is_module_completed(difficultyEnums.difficulty.Plata)
	if(moduloscompletados):
		select_dificulty.set_item_disabled(1,false)
	else:
		select_dificulty.set_item_disabled(1,true)
	moduloscompletados = GameManager.is_module_completed(difficultyEnums.difficulty.Oro)
	if(moduloscompletados):
		select_dificulty.set_item_disabled(2,false)
	else:
		select_dificulty.set_item_disabled(2,true)

#Salir del Juego
func _on_quit_button_pressed() -> void:
	get_tree().quit()
#Mostrar opciones de dificultad
func _on_play_button_pressed() -> void:
	select_dificulty.show()
	timer.start()
#Esconder opciones de dificultad
func _on_timer_timeout() -> void:
	select_dificulty.hide()
#Pasar a la escena correspondiente
func _on_select_dificulty_item_selected(index: int) -> void:
	match index:
		0:
			get_tree().change_scene_to_file("res://Scenes/Bronce.tscn")
		1:
			get_tree().change_scene_to_file("res://Scenes/Plata.tscn")
		2:
			get_tree().change_scene_to_file("res://Scenes/Oro.tscn")
#Mostrar menu de opciones
func _on_options_button_pressed() -> void:
	options_menu.show()
