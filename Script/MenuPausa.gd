extends Control

@onready var options_menu: PanelContainer = %OptionsMenu
@onready var v_box_container: VBoxContainer = $PanelContainer/VBoxContainer
@onready var panel_container: PanelContainer = $PanelContainer


func pausa():
	#get_tree().paused = true
	panel_container.show()

func sacarPausa():
	#get_tree().paused = false
	panel_container.hide()

func mostrarOpciones():
	options_menu.show()

func volveralMenu():
	get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")


func _on_options_menu_visibility_changed() -> void:
	if(options_menu.visible == false):
		panel_container.show()
