using UnityEngine;
using UnityEditor;

public class SoundTool : EditorWindow {

	[MenuItem ("Window/Sound Tools")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		SoundTool window = (SoundTool)EditorWindow.GetWindow (typeof (SoundTool));
		window.Show();
	}
	#if UNITY_4 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6


	int loadtypeInt = 0;
	string [] loadtypeStrings = new string[] { "Decompress On Load", "Compressed In Memory", "Streaming" };
	float compressionkbps = 156;

	void OnGUI () {
		GUILayout.Label ("Sound Tools for Unity4", EditorStyles.boldLabel);
		GUILayout.Label ("Audio Format");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("Native")){
			SelectedToggleCompressionSettings(AudioImporterFormat.Native);
		}
		if(GUILayout.Button ("Compressed")){
			SelectedToggleCompressionSettings(AudioImporterFormat.Compressed);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.Label ("3D Sound");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedToggle3DSoundSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedToggle3DSoundSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.Label ("Force To Mono");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedToggleForceToMonoSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedToggleForceToMonoSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.Label ("Default Settings");
		GUILayout.BeginVertical("box");
		GUILayout.Label ("Load Type");
		loadtypeInt = GUILayout.Toolbar (loadtypeInt, loadtypeStrings );
		if(GUILayout.Button ("Change")){
			switch ( loadtypeInt ) {
			case 0 :
				SelectedLoadType (AudioImporterLoadType.DecompressOnLoad);
				break;
			case 1 :
				SelectedLoadType (AudioImporterLoadType.CompressedInMemory);
				break;
			case 2 :
				SelectedLoadType (AudioImporterLoadType.StreamFromDisc);
				break;
			}
		}
		GUILayout.EndVertical();
		GUILayout.Space(10);

		GUILayout.Label ("Hardware decoding");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			enableHardwareDecoding(true);
		}
		if(GUILayout.Button ("False")){
			enableHardwareDecoding(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.Label ("Gapless looping");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedGaplessLoopingSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedGaplessLoopingSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);

		GUILayout.BeginHorizontal("box");
		compressionkbps = EditorGUILayout.Slider ("Compression(kbps)", compressionkbps, 32, 240);
		if(GUILayout.Button ("Change")){
			SelectedSetCompressionBitrate((int)compressionkbps*1000);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(30);
		GUILayout.Label ("sales@8bitgamesound.com");
	}

	void SelectedToggleCompressionSettings(AudioImporterFormat newFormat) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.format = newFormat;
			AssetDatabase.ImportAsset(path);
			Debug.Log("Changed Audio Format");
		}
	}

	void SelectedToggle3DSoundSettings(bool enabled) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.threeD = enabled;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed 3D Sound");
	}
	
	void SelectedToggleForceToMonoSettings(bool enabled) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.forceToMono = enabled;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Force to mono");
	}

	void SelectedLoadType(AudioImporterLoadType newFormat) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.loadType = newFormat;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Load Type");
	}

	void enableHardwareDecoding ( bool enable ) {
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			if(audioImporter.hardware != enable) {
				audioImporter.hardware = enable;
				AssetDatabase.ImportAsset(path);
			}
		}
		
	}

	void SelectedGaplessLoopingSettings(bool enabled) {
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			if(audioImporter.loopable != enabled) {
				audioImporter.loopable = enabled;
				AssetDatabase.ImportAsset(path);
			}
		}

	}

	void SelectedSetCompressionBitrate(int newCompressionBitrate) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.compressionBitrate = newCompressionBitrate;
			AssetDatabase.ImportAsset(path);
		}
	}

	#elif UNITY_5
	
	int loadtypeInt = 0;
	string [] loadtypeStrings = new string[] { "Decompress On Load", "Compressed In Memory", "Streaming" };
	int compressionInt = 0;
	string [] compressionStrings = new string[] { "PCM", "MP3/Vorbis", "ADPCM" };
	float compressionQuality = 30;
	int samppleRateInt = 0;
	int samppleRateInt2 = 3;
	string [] samppleRateStrings = new string[] { "Preserve Sample Rate", "Optimize Sample Rate", "Override Sample Rate" };
	string [] samppleRateStrings2 = new string[] { "8,000Hz", "11,025Hz", "22,050Hz", "32,000Hz", "44,100Hz", "48,000Hz"};
	
	void OnGUI () {
		GUILayout.Label ("Sound Tools for Unity5", EditorStyles.boldLabel);
		
		
		GUILayout.Label ("Force To Mono");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedToggleForceToMonoSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedToggleForceToMonoSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);
		
		GUILayout.Label ("Load In Background");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedToggleLoadInBackgroundSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedToggleLoadInBackgroundSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);
		
		GUILayout.Label ("Preload Audio Data");
		GUILayout.BeginHorizontal("box");
		if(GUILayout.Button ("True")){
			SelectedTogglePreloadAudioDataSettings(true);
		}
		if(GUILayout.Button ("False")){
			SelectedTogglePreloadAudioDataSettings(false);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10);
		
		GUILayout.Label ("Default Settings");
		GUILayout.BeginVertical("box");
		GUILayout.Label ("Load Type");
		loadtypeInt = GUILayout.Toolbar (loadtypeInt, loadtypeStrings );
		if(GUILayout.Button ("Change")){
			switch ( loadtypeInt ) {
			case 0 :
				SelectedLoadType (AudioClipLoadType.DecompressOnLoad);
				break;
			case 1 :
				SelectedLoadType (AudioClipLoadType.CompressedInMemory);
				break;
			case 2 :
				SelectedLoadType (AudioClipLoadType.Streaming);
				break;
			}
		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		GUILayout.Label ("Compression Format");
		compressionInt = GUILayout.Toolbar (compressionInt, compressionStrings );
		if (compressionInt == 1) {
			compressionQuality = EditorGUILayout.Slider ("Quality", compressionQuality, 1, 100);
		}
		
		if(GUILayout.Button ("Change")){
			switch ( compressionInt ) {
			case 0 :
				SelectedCompressionFormat (AudioCompressionFormat.PCM);
				break;
			case 1 :
				SelectedCompressionFormat (AudioCompressionFormat.Vorbis);
				break;
			case 2 :
				SelectedCompressionFormat (AudioCompressionFormat.ADPCM);
				break;
			}
			
			
		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical("box");
		GUILayout.Label ("Sample Rate Setting");
		samppleRateInt = GUILayout.Toolbar (samppleRateInt, samppleRateStrings );
		if (samppleRateInt == 2) {
			samppleRateInt2 = GUILayout.Toolbar ( samppleRateInt2, samppleRateStrings2 );
		}
		if(GUILayout.Button ("Change")){
			switch ( samppleRateInt ) {
			case 0 :
				SelectedSampleRateSetting (AudioSampleRateSetting.PreserveSampleRate);
				break;
			case 1 :
				SelectedSampleRateSetting (AudioSampleRateSetting.OptimizeSampleRate);
				break;
			case 2 :
				SelectedSampleRateSetting (AudioSampleRateSetting.OverrideSampleRate);
				break;
			}
		}
		GUILayout.EndVertical();
		
		GUILayout.Space(30);
		GUILayout.Label ("iamjuneyoung@gmail.com");
	}
	
	
	void SelectedToggleForceToMonoSettings(bool enabled) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.forceToMono = enabled;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Force to mono");
	}
	
	void SelectedToggleLoadInBackgroundSettings(bool enabled) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.loadInBackground = enabled;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Load in Background");
	}
	
	void SelectedTogglePreloadAudioDataSettings(bool enabled) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.preloadAudioData = enabled;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Pre Load Audio");
	}
	
	void SelectedLoadType(AudioClipLoadType loadType) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		AudioImporterSampleSettings loadSetting = new AudioImporterSampleSettings();
		loadSetting.loadType = loadType;
		
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.defaultSampleSettings = loadSetting;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Load Type");
	}
	
	void SelectedCompressionFormat(AudioCompressionFormat comFormat ) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		AudioImporterSampleSettings loadSetting = new AudioImporterSampleSettings();
		loadSetting.compressionFormat = comFormat;
		loadSetting.quality = compressionQuality/100;
		
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.defaultSampleSettings = loadSetting;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Compression Format");
	}
	
	void SelectedSampleRateSetting(AudioSampleRateSetting sampleRate ) {
		
		Object[] audioclips = GetSelectedAudioclips();
		Selection.objects = new Object[0];
		AudioImporterSampleSettings loadSetting = new AudioImporterSampleSettings();
		loadSetting.sampleRateSetting = sampleRate;
		switch (samppleRateInt2) {
		case 0:
			loadSetting.sampleRateOverride = 8000;
			break;
		case 1:
			loadSetting.sampleRateOverride = 11025;
			break;
		case 2:
			loadSetting.sampleRateOverride = 22050;
			break;
		case 3:
			loadSetting.sampleRateOverride = 32000;
			break;
		case 4:
			loadSetting.sampleRateOverride = 44100;
			break;
		case 5:
			loadSetting.sampleRateOverride = 48000;
			break;
		}
		
		foreach (AudioClip audioclip in audioclips) {
			string path = AssetDatabase.GetAssetPath(audioclip);
			AudioImporter audioImporter = AssetImporter.GetAtPath(path) as AudioImporter;
			audioImporter.defaultSampleSettings = loadSetting;
			AssetDatabase.ImportAsset(path);
		}
		Debug.Log("Changed Sample Rate");
	}


	#endif

	static Object[] GetSelectedAudioclips()	{
		return Selection.GetFiltered(typeof(AudioClip), SelectionMode.DeepAssets);
	}

}
