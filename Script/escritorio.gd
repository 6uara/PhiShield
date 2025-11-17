# Desktop.gd
extends Control
class_name Escritorio
@export var current_difficulty: difficultyEnums.difficulty = difficultyEnums.difficulty.Bronce

# --- NODOS DE LA INTERFAZ ---
@onready var remitente_label: RichTextLabel = %Remitente
@onready var asunto_label: Label = %Asunto
@onready var cuerpo_email: RichTextLabel = %CuerpoMail
@onready var adjuntos_box: HBoxContainer = %AdjuntosContainer
@onready var tooltip_container: PanelContainer = $TooltipContainer
@onready var tooltip: Label = %Tooltip

# --- NODOS DE DECISIÓN ---
@onready var boton_confiar: Button = %NoPhishingButton
@onready var boton_reportar: Button = %PhishingButton
@onready var siguiente_mail: Button = %SiguienteMail

# --- NODOS DE HERRAMIENTAS Y POPUPS ---
@onready var boton_inspeccionar_remitente: Button = %VerificarDominio
@onready var boton_escanear_adjuntos: Button = %EscanearAdjunto
@onready var boton_manual: Button = %Help

@onready var tutorial: Tutorial = %Tutorial

@onready var info_popup: PopupPanel = %Popup

signal showpopupinfo(text : String)
signal showpoputexthelp

# --- VARIABLES DEL JUEGO ---
var email_actual: EmailData
var speed : float = 18
# -----------------------------------------------------------------
# INICIO
# -----------------------------------------------------------------
func _ready() -> void:
	GameManager.start_module_trial(current_difficulty)
	tutorial.tutorialFinished.connect(makevisible)
	GameManager.trial_finished.connect(stop)

# -----------------------------------------------------------------
# FUNCIONES PRINCIPALES DEL JUEGO
# -----------------------------------------------------------------
func cargar_siguiente_email() -> void:
	email_actual = EmailGenerator.get_next_email(current_difficulty)

	remitente_label.text = "De: " + email_actual.remitente_visible 
	asunto_label.text = "Asunto: " + email_actual.asunto
	cuerpo_email.text = email_actual.cuerpo
	animateRichText(remitente_label)
	animateText(asunto_label)
	animateRichText(cuerpo_email)
	
	for child in adjuntos_box.get_children():
		child.queue_free()
	for adjunto_nombre in email_actual.adjuntos:
		var label = Label.new()
		label.text = adjunto_nombre
		adjuntos_box.add_child(label)
		animateText(label)

	if(boton_confiar.disabled || boton_reportar.disabled):
		activarBotones()
		siguiente_mail.disabled = true

func _on_decision_tomada(decision_del_jugador: bool) -> void:
	var fue_correcto = (decision_del_jugador == email_actual.es_phishing)
	
	if fue_correcto:
		GameManager.record_attempt(true)
		var tipo_email = "LEGÍTIMO"
		if email_actual.es_phishing:
			tipo_email = "PHISHING"
		var feedback = "Correcto!! \n" + "Ese Mail era " + tipo_email
		info_popup.show_info(feedback)
	else:
		GameManager.record_attempt(false)
		var tipo_email = "LEGÍTIMO"
		if email_actual.es_phishing:
			tipo_email = "PHISHING"
		var feedback = "ERROR: Este email era " + tipo_email 
		# Añadir las pistas solo si era phishing y fallaste
		if email_actual.es_phishing and not email_actual.pistas.is_empty():
			feedback += "\nPistas perdidas:\n"
			for pista in email_actual.pistas:
				feedback += "- " + pista + "\n"
		info_popup.show_info(feedback)
	siguiente_mail.disabled = false
	desactivarBotones()

# -----------------------------------------------------------------
# FUNCIONES DE LAS HERRAMIENTAS
# -----------------------------------------------------------------
func _on_link_inspeccionado(_meta: Variant) -> void:
	GameManager.record_attempt(false)
	var feedback = "ERROR: Clickeaste un Link Sospechoso. \n\n"
	showpopupinfo.emit(feedback)
	desactivarBotones()
	siguiente_mail.disabled = false

func _on_inspeccionar_remitente() -> void:
	var info_texto = "Verificando remitente...\n\n"
	info_texto += "Remitente visible: " + email_actual.remitente_visible + "\n"

	info_popup.show_info(info_texto)

func _on_escanear_adjuntos() -> void:
	if email_actual.adjuntos.is_empty():
		info_popup.show_info("No hay archivos adjuntos para escanear.")
		return

	var info_texto = "Escaneando adjuntos...\n\nArchivos encontrados:\n"
	for adjunto in email_actual.adjuntos:
		info_texto += "- " + adjunto + "\n"
		
		if adjunto.to_lower().ends_with(".exe") or \
		   adjunto.to_lower().ends_with(".zip") or \
		   adjunto.to_lower().ends_with(".html"):
			info_texto += "  ¡PELIGRO! Detectada extensión sospechosa.\n"
		elif ".pdf.exe" in adjunto.to_lower():
			info_texto += "  ¡PELIGRO! Detectada doble extensión maliciosa.\n"
	
	info_popup.show_info(info_texto)

func _on_help_asked():
	showpoputexthelp.emit()

func animateRichText(text : RichTextLabel):
	text.visible_characters = 0
	var tween = create_tween()
	tween.tween_property(text, "visible_characters" , text.text.length(), text.text.length() / (speed *4))
	await tween.finished
	return

func animateText(text : Label):
	text.visible_characters = 0
	var tween = create_tween()
	tween.tween_property(text, "visible_characters" , text.text.length(), text.text.length() / speed)
	await tween.finished
	return

func activarBotones():
	boton_confiar.disabled = false
	boton_reportar.disabled = false
	boton_escanear_adjuntos.disabled = false
	boton_inspeccionar_remitente.disabled = false
	boton_manual.disabled = false

func desactivarBotones():
	boton_confiar.disabled = true
	boton_reportar.disabled = true
	boton_escanear_adjuntos.disabled = true
	boton_inspeccionar_remitente.disabled = true
	boton_manual.disabled = true

func makevisible():
	self.visible = true

func stop():
	siguiente_mail.disabled = true
	desactivarBotones()

func hover(meta : Variant, init : bool):
	if(init):
		var text = str(meta).replace("meta=","").replace("{","").replace("}","")
		tooltip.text = text
		tooltip_container.position = get_local_mouse_position()
		tooltip_container.show()
	else:
		tooltip_container.hide()
