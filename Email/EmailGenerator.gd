# EmailGenerator.gd
extends Node
@export var database: EmailTemplates = preload("res://Email/Templates/EmailTemplates.tres")

var email_pools: Dictionary = {}
var current_indices: Dictionary = {}

func _ready() -> void:
	if not database: return
	init()

func get_next_email(difficulty: difficultyEnums.difficulty) -> EmailData:
	if not email_pools.has(difficulty): return null
	
	var pool: Array = email_pools[difficulty]
	if pool.is_empty():	return null
		
	var index: int = current_indices[difficulty]
	if index >= pool.size():
		pool.shuffle()
		index = 0
	
	var email_to_send: EmailData = pool[index]
	current_indices[difficulty] = index + 1
	
	return email_to_send

func init():
	# Usamos el enum para inicializar
	email_pools[difficultyEnums.difficulty.Bronce] = database.BronceTemplates.duplicate()
	email_pools[difficultyEnums.difficulty.Plata] = database.PlataTemplates.duplicate()
	email_pools[difficultyEnums.difficulty.Oro] = database.OroTemplates.duplicate()

	current_indices[difficultyEnums.difficulty.Bronce] = 0
	current_indices[difficultyEnums.difficulty.Plata] = 0
	current_indices[difficultyEnums.difficulty.Oro] = 0

	# 3. Barajar cada mazo
	email_pools[difficultyEnums.difficulty.Bronce].shuffle()
	email_pools[difficultyEnums.difficulty.Plata].shuffle()
	email_pools[difficultyEnums.difficulty.Oro].shuffle()
	
	print("Generador de Emails listo. Emails cargados:")
	print("  Bronce: ", email_pools[difficultyEnums.difficulty.Bronce].size())
	print("  Plata: ", email_pools[difficultyEnums.difficulty.Plata].size())
	print("  Oro: ", email_pools[difficultyEnums.difficulty.Oro].size())
