# InfoPopup.gd
extends PopupPanel
class_name InfoPopUp

@onready var info_label: Label = $TextoPopUp
@onready var close_button: Button = $CerrarPopUp
@onready var timer: Timer = $Timer

@export var helptext : String
@export var popuptr : Node2D

var speed = 18

func _ready() -> void:
	close_button.pressed.connect(hide)


## Esta es la función clave que llamamos desde Desktop.gd
func show_info(texto: String) -> void:
	info_label.text = texto
	popup(Rect2i(popuptr.position.x,popuptr.position.y,250,260))
	timer.start()

func show_help_info() -> void:
	info_label.text = helptext
	popup(Rect2i(popuptr.position.x,popuptr.position.y,250,100))
	timer.start()


func _on_timer_timeout() -> void:
	self.hide()
