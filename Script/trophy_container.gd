extends HBoxContainer
class_name trophyContainer

@onready var bronce: TextureRect = $Bronce
@onready var plata: TextureRect = $Plata
@onready var oro: TextureRect = $Oro


func _ready() -> void:
	if(not GameManager.is_module_completed(difficultyEnums.difficulty.Bronce)):
		bronce.self_modulate -= Color(-50,-50,-50)
	if(not GameManager.is_module_completed(difficultyEnums.difficulty.Plata)):
		plata.self_modulate -= Color(-50,-50,-50)
	if(not GameManager.is_module_completed(difficultyEnums.difficulty.Oro)):
		oro.self_modulate -= Color(-50,-50,-50)
