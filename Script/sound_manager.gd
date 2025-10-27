# AudioManager.gd
extends Node
class_name AudioManagerScript
@onready var master: AudioStreamPlayer = %Master
@onready var vfx: AudioStreamPlayer = %VFX
@onready var music: AudioStreamPlayer = %Music


const AUDIO_LIBRARY = preload("res://Audio/SoundLibrary.tres")
const MASTER_BUS_NAME = "Master"
const MUSIC_BUS_NAME = "Music"
const SFX_BUS_NAME = "SFX"

# --- FUNCIONES DE REPRODUCCIÓN ---

func play_music():
	var rng = RandomNumberGenerator.new()
	var n = rng.randi_range(0,AUDIO_LIBRARY.music.size()-1)
	music.stream = AUDIO_LIBRARY.music[n]
	music.play()

func stop_music(): music.stop()

func play_sfx_limited(stream: AudioStream):
	vfx.stream = stream
	vfx.play()

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
