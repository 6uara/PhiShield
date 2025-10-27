extends Control
class_name Tutorial

@onready var textoTutorial: RichTextLabel = %RichTextLabel

signal tutorialFinished

var tutotext : Array[String]
var _index = 0
var speed = 18
func _ready() -> void:
	populate()

func populate():
	var text = "Asunto: ¡Bienvenido a tu primer día, Analista! 
	¡Hola! Eres nuestra nueva línea de defensa. 
	Tu misión es proteger a nuestra organización de la amenaza digital más común y peligrosa: el Phishing."
	tutotext.append(text)
	text = "¿Qué es el Phishing? 
	En pocas palabras: es una estafa.
	Los ciberdelincuentes se hacen pasar por alguien de confianza (tu banco, un servicio técnico, una red social o incluso un colega de trabajo) y te envían un correo.
	Su objetivo es simple: engañarte para que hagas clic en un enlace malicioso o descargues un archivo peligroso."
	tutotext.append(text)
	text = "¿Por qué es tan peligroso? 
	Para las personas: Un solo clic puede terminar en el robo de tu identidad, la pérdida de dinero de tus cuentas bancarias o el secuestro de tus redes sociales.
	Para la empresa: Una sola cuenta comprometida puede llevar a una brecha de seguridad masiva, el robo de datos de millones de clientes y pérdidas financieras irreparables.
	Tu trabajo es que eso no suceda."
	tutotext.append(text)
	text = "Tu Misión: Análisis 
	En esta primera tanda de correos, tu objetivo es simple. 
	No buscaremos trucos complejos. Nos enfocaremos en las dos mentiras más comunes.
	La regla de oro es: NUNCA CONFÍES EN LO QUE VES A SIMPLE VISTA.
	Los atacantes mienten en dos lugares clave:"
	tutotext.append(text)
	text = "EL REMITENTE (El 'De'):
		El nombre puede decir 'Soporte Banco Global'.
		La dirección real puede ser soporte.banco@cuenta-gratis123.xyz.
		Siempre revisa el mail del remitente para verificar que concuerde con el oficial. 
		estate atento a cualquier pequena diferencia ya que el objetivo es que creas que son el real."
	tutotext.append(text)
	text= "LOS ENLACES (Los 'Links'):
		El texto del enlace puede decir bancoglobal.com/login.
		Pero el enlace real puede apuntar a bancogIobal-seguro.com (¡fíjate en la 'i' mayúscula!).
		En estos casos se debe revisar que el dominio concuerde con la direccion oficial de la entidad"
	tutotext.append(text)
	text = "Cómo usar tus herramientas:
		Para cada correo que recibas, deberás usar tus herramientas de análisis antes de tomar una decisión:
		[Verificar Dominio]: Úsala para ver la dirección de correo real del remitente. 
		[Inspeccionar Enlace]: Haz clic en cualquier enlace del correo. Esto no lo abrirá, sino que te mostrará su destino real.
		[Manual]: Consulta tu manual. ¿Coincide el dominio real con el dominio oficial de la empresa?Decide: 
		Si todo es correcto, presiona [No es Phishing]. Si algo es sospechoso, presiona [Es Phishing].
		Estás listo para empezar. Abre tu bandeja de entrada y comencemos.¡Presta atención a los detalles!"
	tutotext.append(text)
	text = "Si todo es correcto, presiona [No es Phishing]. Si algo es sospechoso, presiona [Es Phishing].
		Estás listo para empezar. Abre tu bandeja de entrada y comencemos.¡Presta atención a los detalles!"
	tutotext.append(text)

func getText():
	if(_index < tutotext.size()):
		var texto = tutotext[_index]
		textoTutorial.visible_characters = 0
		textoTutorial.text = texto
		animateText()
		_index += 1
	else:  
		print("No more Text")
		tutorialFinished.emit()
		self.visible = false


func animateText():
	var tween = create_tween()
	tween.tween_property(textoTutorial, "visible_characters" , textoTutorial.text.length(), textoTutorial.text.length() / speed)
