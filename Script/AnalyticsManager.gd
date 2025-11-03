# AnalyticsManager.gd
extends Node
# --- ¡CONFIGURA ESTO! ---
# Pega aquí tus datos de Google Analytics
const MEASUREMENT_ID = "G-J7FZED8YRC" 
const API_SECRET = "Cqdjqg5LRoye9-1E7fvZAA"
# -------------------------

const GA_URL = "https://www.google-analytics.com/mp/collect?measurement_id=%s&api_secret=%s"
const CLIENT_ID_SAVE_PATH = "user://client.id"

var client_id: String
var http_request: HTTPRequest

func _ready():
	# 1. Crear el nodo HTTPRequest en tiempo de ejecución
	http_request = HTTPRequest.new()
	add_child(http_request)
	# (Opcional) Conectar la señal si quieres depurar la respuesta
	http_request.request_completed.connect(_on_request_completed)

	# 2. Obtener o crear el ID de cliente único
	client_id = _get_or_create_client_id()
	print("AnalyticsManager listo. ID de Cliente: ", client_id)


## -----------------------------------------------------------------
## API PÚBLICA: ¡Llama a esta función desde cualquier script!
## -----------------------------------------------------------------

## Envía un evento a Google Analytics.
## @event_name: El nombre de tu evento (ej. "level_start", "phishing_failed")
## @event_params: Un diccionario con datos extra (ej. {"level_name": "Bronce", "score": 100})
func send_event(event_name: String, event_params: Dictionary = {}):
	
	# 1. Construir la URL completa
	var url = GA_URL % [MEASUREMENT_ID, API_SECRET]
	
	# 2. Definir las cabeceras (Headers)
	var headers = ["Content-Type: application/json"]
	
	# 3. Construir el cuerpo (Body) de la petición
	var body_dict = {
		"client_id": client_id,
		"events": [
			{
				"name": event_name,
				"params": event_params
			}
		]
	}
	
	# Convertir el diccionario a un string JSON
	var body_json = JSON.stringify(body_dict)
	
	# 4. Enviar la petición (¡no bloqueante!)
	# El juego no se congelará, enviará esto en segundo plano.
	var error = http_request.request(url, headers, HTTPClient.METHOD_POST, body_json)
	
	if error != OK:
		printerr("AnalyticsManager: Error al iniciar la petición HTTP: ", error)


## -----------------------------------------------------------------
## LÓGICA INTERNA (ID de Cliente)
## -----------------------------------------------------------------

func _get_or_create_client_id() -> String:
	# 1. Intentar cargar el ID guardado
	if FileAccess.file_exists(CLIENT_ID_SAVE_PATH):
		var file = FileAccess.open(CLIENT_ID_SAVE_PATH, FileAccess.READ)
		var id = file.get_line()
		file.close()
		if not id.is_empty():
			return id
			
	# 2. Si no existe, crear uno nuevo
	# Generamos un ID "único" simple usando el tiempo y un número aleatorio
	var new_id = "user_%s_%s" % [Time.get_ticks_usec(), randi() % 9999]
	
	# 3. Guardar el nuevo ID
	var file = FileAccess.open(CLIENT_ID_SAVE_PATH, FileAccess.WRITE)
	file.store_line(new_id)
	file.close()
	
	return new_id


## (Opcional) Para depurar la respuesta de Google
func _on_request_completed(result, response_code, headers, body):
	if response_code == 204:
		print("AnalyticsManager: Evento enviado con éxito.")
	else:
		printerr("AnalyticsManager: Error al enviar evento. Código: %s" % response_code)
		print("Respuesta de Google: ", body.get_string_from_utf8())
