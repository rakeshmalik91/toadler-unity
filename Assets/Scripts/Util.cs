using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {

	public static Color SetAlpha(float a, Color c) {
		c.a = a;
		return c;
	}
}
