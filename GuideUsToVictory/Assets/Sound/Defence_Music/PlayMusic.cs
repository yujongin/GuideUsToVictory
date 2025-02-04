using UnityEngine;
using System.Collections;

public class PlayMusic : MonoBehaviour {
	public AudioClip m1_1, m1_2, m2_1, m2_2, m3_1, m3_2, mBoss, mMap, mLose, mWinIntro, mWinLoop;

	private AudioSource[] pMusics;
	
	// Use this for initialization
	void Start () {
		pMusics = GetComponents<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		int sizeX = 100;
		int sizeY = 30;
		int defX = 100;
		int defY = 50;
	
		GUI.color= Color.black;

		GUI.Label ( new Rect (defX * 0 , defY * 0 , sizeX, sizeY ), "1_1_Normal" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 0 , sizeX, sizeY ), "Play") ){
			MusicPlay(m1_1, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 0 , sizeX, sizeY ), "Stop") ){
			MusicStop (m1_1);
		}

		GUI.Label ( new Rect (defX * 4 , defY * 0 , sizeX, sizeY ), "1_2_Attack" );
		if ( GUI.Button ( new Rect (defX * 5, defY * 0 , sizeX, sizeY ), "Play") ){
			MusicPlay(m1_2, true);
		}
		if ( GUI.Button ( new Rect (defX * 6, defY * 0 , sizeX, sizeY ), "Stop") ){
			MusicStop (m1_2);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 1 , sizeX, sizeY ), "2_1_Normal" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 1 , sizeX, sizeY ), "Play") ){
			MusicPlay(m2_1, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 1 , sizeX, sizeY ), "Stop") ){
			MusicStop (m2_1);
		}
		
		GUI.Label ( new Rect (defX * 4 , defY * 1 , sizeX, sizeY ), "2_2_Attack" );
		if ( GUI.Button ( new Rect (defX * 5, defY * 1 , sizeX, sizeY ), "Play") ){
			MusicPlay(m2_2, true);
		}
		if ( GUI.Button ( new Rect (defX * 6, defY * 1 , sizeX, sizeY ), "Stop") ){
			MusicStop (m2_2);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 2 , sizeX, sizeY ), "3_1_Normal" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 2 , sizeX, sizeY ), "Play") ){
			MusicPlay(m1_1, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 2 , sizeX, sizeY ), "Stop") ){
			MusicStop (m1_1);
		}
		
		GUI.Label ( new Rect (defX * 4 , defY * 2 , sizeX, sizeY ), "3_2_Attack" );
		if ( GUI.Button ( new Rect (defX * 5, defY * 2 , sizeX, sizeY ), "Play") ){
			MusicPlay(m1_2, true);
		}
		if ( GUI.Button ( new Rect (defX * 6, defY * 2 , sizeX, sizeY ), "Stop") ){
			MusicStop (m1_2);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 3 , sizeX, sizeY ), "Boss" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 3 , sizeX, sizeY ), "Play") ){
			MusicPlay(mBoss, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 3 , sizeX, sizeY ), "Stop") ){
			MusicStop (mBoss);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 4 , sizeX, sizeY ), "Map" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 4 , sizeX, sizeY ), "Play") ){
			MusicPlay(mMap, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 4 , sizeX, sizeY ), "Stop") ){
			MusicStop (mMap);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 5 , sizeX, sizeY ), "Lose" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 5 , sizeX, sizeY ), "Play") ){
			MusicPlay(mLose, true);
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 5 , sizeX, sizeY ), "Stop") ){
			MusicStop (mLose);
		}

		GUI.Label ( new Rect (defX * 0 , defY * 6 , sizeX, sizeY ), "Win" );
		if ( GUI.Button ( new Rect (defX * 1, defY * 6 , sizeX, sizeY ), "Play") ){
			StartCoroutine ( MusicPlay () );
		}
		if ( GUI.Button ( new Rect (defX * 2, defY * 6 , sizeX, sizeY ), "Stop") ){
			MusicStop (mWinLoop);
			MusicStop (mWinIntro);
		}

	}

	IEnumerator MusicPlay () {
		MusicPlay(mWinIntro, false);
		yield return new WaitForSeconds ( mWinIntro.length );
		MusicPlay2(mWinLoop, true);

	}

	void MusicPlay ( AudioClip mName, bool mloop ) {
		if (pMusics [0].clip != null) {
			pMusics[0].clip = null;
		}
		pMusics[0].clip = mName;
		pMusics[0].loop = mloop;
		pMusics[0].Play ();
	}

	void MusicPlay2 ( AudioClip mName, bool mloop ) {
		if (pMusics [1].clip != null) {
			pMusics[1].clip = null;
		}
		pMusics[1].clip = mName;
		pMusics[1].loop = mloop;
		pMusics[1].Play ();
	}

	void MusicStop (AudioClip mName) {
		for( int i=0; i < pMusics.Length; i++ ) {
			if(pMusics[i].clip != null){
				pMusics[i].Stop ();
			}
		}
	}

	IEnumerator EndMusic ( AudioClip mName ) {
		yield return new WaitForSeconds ( mName.length + 0.2f );
		MusicStop ( mName ) ;

		// remove Music
	}


}
