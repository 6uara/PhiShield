extends Control

@onready var encuesta_ui: VBoxContainer = $PanelContainer/EncuestaUI
@onready var encuesta_ux: VBoxContainer = $PanelContainer/EncuestaUX
@onready var encuesta_usabilidad: VBoxContainer = $PanelContainer/EncuestaUsabilidad
@onready var encuesta_general: VBoxContainer = $PanelContainer/EncuestaGeneral

var lista_de_encuestas : Array[Variant]
var index = 0

func _ready() -> void:
	lista_de_encuestas.append(encuesta_ui)
	lista_de_encuestas.append(encuesta_ux)
	lista_de_encuestas.append(encuesta_usabilidad)
	lista_de_encuestas.append(encuesta_general)
	lista_de_encuestas[index].show()
	index += 1


func siguiente():
	if index != 0:
		lista_de_encuestas[index - 1].hide()
	if index >= lista_de_encuestas.size():
		get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")
	else:
		lista_de_encuestas[index].show()
		index += 1
	
