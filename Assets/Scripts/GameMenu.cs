using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameMenu : MonoBehaviour {

	public static bool paused;

	public GameObject frog;
	public GameObject lilypadGenerator;

	public static float startTime;

	void Start () {
		paused = true;
		InitGPGS ();
	}

	public void SetPause(bool value) {
		paused = value;
		GameObject.Find ("ButtonPlay").GetComponent<UnityEngine.UI.Button> ().interactable = true;
		GameObject.Find ("ButtonPlay").transform.GetChild (0).gameObject.SetActive (false);
		GameObject.Find ("ButtonPlay").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().text = "Continue";
		GameObject.Find ("ButtonPlay").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color = Util.SetAlpha(1f, GameObject.Find ("ButtonPlay").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color);
		GameObject.Find ("ButtonRetry").GetComponent<UnityEngine.UI.Button> ().interactable = true;
		GameObject.Find ("ButtonRetry").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color = Util.SetAlpha(1f, GameObject.Find ("ButtonRetry").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color);

		if (!paused && frog.GetComponent<Frog>().IsDead ()) {
			Life.life--;
			Life.lifeUsedInCurrentGame++;
		}

		if (Score.score == 0) {
			startTime = Time.time;
		}

		SaveGame ();

		ReportGPGSProgress ();
	}

	public void Exit() {
		SaveGame ();

		ReportGPGSProgress ();

		Application.Quit ();
	}

	public void Retry() {
		frog.GetComponent<Frog>().Reset();
		lilypadGenerator.GetComponent<LilypadGenerator>().Reset();
		Score.score = 0;
		Life.lifeUsedInCurrentGame = 0;
		paused = false;

		ShowAd ();

		startTime = Time.time;
	}



	//---------------------------------------------------- Save Game ----------------------

	public static void SaveGame() {
		PlayerPrefs.SetInt ("life", Life.life);
		PlayerPrefs.SetInt ("highScore", HighScore.highScore);
		PlayerPrefs.SetFloat ("audio", AudioListener.volume);
		PlayerPrefs.Save ();
	}



	//---------------------------------------------------- Unity Ads Services ----------------------

	public void ShowAd() {
		if (Random.Range (0f, 1f) < 0.2f && Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
	}

	public void ShowRewardedAd() {
		if (Advertisement.IsReady ("rewardedVideo")) {
			var options = new ShowOptions { resultCallback = HandleShowAdResult };
			Advertisement.Show ("rewardedVideo", options);
		} else {
			UnityEngine.UI.Text error = GameObject.Find ("TextVideoError").GetComponent <UnityEngine.UI.Text>();
			error.color = new Color (error.color.r, error.color.g, error.color.b, 1f);
		}

		if (GameObject.Find ("ButtonRetry").GetComponent<UnityEngine.UI.Button> ().interactable) {
			GameObject.Find ("ButtonPlay").GetComponent<UnityEngine.UI.Button> ().interactable = false;
			GameObject.Find ("ButtonPlay").transform.GetChild (0).gameObject.SetActive (false);
			GameObject.Find ("ButtonPlay").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color = Util.SetAlpha(0.5f, GameObject.Find ("ButtonPlay").transform.FindChild ("Text").GetComponent<UnityEngine.UI.Text> ().color);
		}
	}

	private void HandleShowAdResult(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			Life.life++;
			ReportGPGSProgress ();
			Life.lifeUsedInCurrentGame = 0;
			Life.lifeCollectedInCurrentGame = 0;
			PlayerPrefs.SetInt ("life", Life.life);
			PlayerPrefs.Save ();
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
	}



	//---------------------------------------------------- Google Play Games Services ----------------------

	private static bool authenticatedGPGS = false;

	public static void InitGPGS() {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.RequireGooglePlus()
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = false;
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate((bool success) => {
			if(success) {
				Debug.Log("Successfully signed in to Google Play Services");
				authenticatedGPGS = true;
				ReportGPGSProgress();
			} else {
				Debug.Log("Could not sign in to Google Play Services");
				GameObject.Find ("ButtonAchievements").SetActive(false);
				GameObject.Find ("ButtonLeaderboard").SetActive(false);
			}
		});
	}

	public static void ReportGPGSProgress() {
		if (!authenticatedGPGS) {
			return;
		}

		//GPGS achievements
		SetAchievementProgress (GPGSConstants.achievement_toddler_toadler, HighScore.highScore, 0, 100);
		SetAchievementProgress (GPGSConstants.achievement_young_toadler, HighScore.highScore, 100, 250);
		SetAchievementProgress (GPGSConstants.achievement_adult_toadler, HighScore.highScore, 250, 500);
		SetAchievementProgress (GPGSConstants.achievement_experienced_toadler, HighScore.highScore, 500, 1000);

		SetAchievementProgress (GPGSConstants.achievement_sturdy, Life.lifeUsedInCurrentGame, 0, 2);
		SetAchievementProgress (GPGSConstants.achievement_resilient, Life.lifeUsedInCurrentGame, 2, 10);
		SetAchievementProgress (GPGSConstants.achievement_determined, Life.lifeUsedInCurrentGame, 10, 25);

		SetAchievementProgress (GPGSConstants.achievement_collector, Life.life, 0, 100);

		SetAchievementProgress (GPGSConstants.achievement_lucky, Life.lifeCollectedInCurrentGame, 0, 10);

		//GPGS loaderboard
		Social.ReportScore(HighScore.highScore, GPGSConstants.leaderboard_high_score, (bool success) => {});
		if (Score.score200Duration > 0) {
			Social.ReportScore (Score.score200Duration, GPGSConstants.leaderboard_time_for_200, (bool success) => {});
		}
		if (Score.score500Duration > 0) {
			Social.ReportScore (Score.score500Duration, GPGSConstants.leaderboard_time_for_500, (bool success) => {});
		}
	}

	private static void SetAchievementProgress(string achId, int newProgress, int min, int max) {
		if(newProgress >= min && newProgress <= max) {
			int currentProgress = PlayGamesPlatform.Instance.GetAchievement (achId).CurrentSteps;
			if(newProgress > currentProgress) {
				//Reveal if not revealed yet
				if(!PlayGamesPlatform.Instance.GetAchievement (achId).IsRevealed) {
					Social.ReportProgress (achId, 0f, (bool success) => {
						if(success) {
							Debug.Log("Achievement " + achId + " revealed.");
						}
					});
				}
				//Set progress
				int increment = newProgress - currentProgress;
				PlayGamesPlatform.Instance.IncrementAchievement(achId, increment, (bool success) => {
					if(success) {
						Debug.Log("Achievement " + achId + " progress set to " + newProgress + ".");
					}
				});
			}
		}
	}

	public void ShowAchievementsUI() {
		Social.ShowAchievementsUI ();
	}

	public void ShowLeaderboardUI() {
		Social.ShowLeaderboardUI ();
	}
}
