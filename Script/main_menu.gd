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

func _on_quit_button_pressed() -> void:
	get_tree().quit()

func _on_play_button_pressed() -> void:
	select_dificulty.show()
	timer.start()

func _on_timer_timeout() -> void:
	select_dificulty.hide()

func _on_select_dificulty_item_selected(index: int) -> void:
	match index:
		0: get_tree().change_scene_to_file("res://Scenes/Modulos/Bronce.tscn")
		1: get_tree().change_scene_to_file("res://Scenes/Modulos/Plata.tscn")
		2: get_tree().change_scene_to_file("res://Scenes/Modulos/Oro.tscn")

func _on_options_button_pressed() -> void:
	options_menu.show()


func _on_encuesta_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/encuesta.tscn")
