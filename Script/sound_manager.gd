# AudioManager.gd
extends Node

@onready var master_audio: AudioStreamPlayer = %MasterAudio
@onready var musci_audio: AudioStreamPlayer = %MusciAudio
@onready var sfx_audio: AudioStreamPlayer = %SFXAudio

const MASTER_BUS_NAME = "Master"
const MUSIC_BUS_NAME = "Music"
const SFX_BUS_NAME = "SFX"

# --- FUNCIONES DE REPRODUCCIÓN ---

func play_music(stream: AudioStream):
	musci_audio.stream = stream
	musci_audio.play()

func stop_music(): musci_audio.stop()

func play_sfx_limited(stream: AudioStream):
	sfx_audio.stream = stream
	sfx_audio.play()

func play_sfx(stream: AudioStream):
	var sfx_player = AudioStreamPlayer.new()
	sfx_player.stream = stream
	sfx_player.bus = SFX_BUS_NAME 
	sfx_player.finished.connect(sfx_player.queue_free)
	add_child(sfx_player)
	sfx_player.play()

func set_master_volume_db(db: float): AudioServer.set_bus_volume_db(AudioServer.get_bus_index(MASTER_BUS_NAME), db)

func set_music_volume_db(db: float): AudioServer.set_bus_volume_db(AudioServer.get_bus_index(MUSIC_BUS_NAME), db)

func set_sfx_volume_db(db: float): AudioServer.set_bus_volume_db(AudioServer.get_bus_index(SFX_BUS_NAME), db)

func set_music_volume_percent(percent: float):
	var db = linear_to_db(percent)
	set_music_volume_db(db)
