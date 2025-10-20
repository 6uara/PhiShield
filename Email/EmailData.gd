# EmailData.gd
class_name EmailData
extends Resource

## El email que ve el jugador
@export var remitente_visible: String
## El email real (para la trampa)
@export var remitente_real: String
@export var asunto: String

## El cuerpo del email, ¡usando BBCode!
@export_multiline var cuerpo: String 

## Un array de diccionarios para los enlaces
# Ej: [{"texto_visible": "aquí", "url_real": "http://banc0.xyz"}]
@export var enlaces: Array[Dictionary] 

## Un array de strings para los adjuntos
# Ej: ["factura.pdf", "reporte.zip.exe"]
@export var adjuntos: Array[String] 

## --- La respuesta correcta ---
@export var es_phishing: bool
@export var pistas: Array[String] # Ej: ["Dominio falso", "Sentido de urgencia"]
