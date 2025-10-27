extends PanelContainer

@onready var texto_eo_modulo: RichTextLabel = $VBoxContainer/TextoEoModulo
@onready var siguiente: Button = $VBoxContainer/HBoxContainer/Siguiente
@onready var reintentar: Button = $VBoxContainer/HBoxContainer/Reintentar

var speed = 18 

func _ready() -> void:
	GameManager.trial_finished.connect(trialfinished)

func trialfinished(paso : bool, dificultadsiguiente : Variant):
	print("Se llamo al eom")
	self.show()
	if(paso):
		print("Paso")
		texto_eo_modulo.text = "Felicitaciones!!
		Has completado con exito el modulo " + difficultyEnums.difficulty.keys()[dificultadsiguiente]
		animateRichText(texto_eo_modulo)
	else:
		print("No Paso")
		texto_eo_modulo.text = "Oh no!! has fallado este intento. 
		Pero no te preocupes, siempre podes intentar de nuevo.
		Recuerda repasar los conceptos aprendidos."
		animateRichText(texto_eo_modulo)
		siguiente.disabled = true

func animateRichText(text : RichTextLabel):
	text.visible_characters = 0
	var tween = create_tween()
	tween.tween_property(text, "visible_characters" , text.text.length(), text.text.length() / (speed *2))
	await tween.finished
	return

func volveralmenu():
	get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")

func siguientemodulo():
	match GameManager.current_module:
		#difficultyEnums.difficulty.Bronce:
			#get_tree().change_scene_to_file("res://Scenes/Plata.tscn")
		#difficultyEnums.difficulty.Plata:
			#get_tree().change_scene_to_file("res://Scenes/Oro.tscn")
		difficultyEnums.difficulty.Oro:
			get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")
		_:
			get_tree().change_scene_to_file("res://Scenes/main_menu.tscn")

func reintentarmodulo():
	match GameManager.current_module:
		difficultyEnums.difficulty.Bronce:
			get_tree().change_scene_to_file("res://Scenes/Bronce.tscn")
		difficultyEnums.difficulty.Plata:
			get_tree().change_scene_to_file("res://Scenes/Plata.tscn")
		difficultyEnums.difficulty.Oro:
			get_tree().change_scene_to_file("res://Scenes/Oro.tscn")
