using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvatarReferences{
	public Renderer avatarRenderer;
	public Renderer viewLine;
	public Renderer markerCircle;
	public Renderer markerCircleSphere;
    public TextMesh naming;
	public Transform headEffector;
}
	
public class AvatarSettings : MonoBehaviour {
	public AvatarReferences avatarReferences;
	public string AvatarName;
	public Color mainColor;
	public Color outlineColor;


	public void setAvatarName(string avatarName){
        avatarReferences.naming.text = avatarName;
	}

	public void setAvatarColor(Color main,Color outline){
		//material - sharedMaterial
		avatarReferences.avatarRenderer.material.SetColor ("_Color", main);
		//avatarReferences.avatarRenderer.material.SetColor ("_OutlineColor", outline);

		avatarReferences.viewLine.material.color = main;
		avatarReferences.viewLine.material.SetColor("_EmissionColor",main);

		avatarReferences.markerCircle.material.color = main;
		avatarReferences.markerCircle.material.SetColor("_EmissionColor",main);

		avatarReferences.markerCircleSphere.material.color = main;
		avatarReferences.markerCircleSphere.material.SetColor("_EmissionColor",main);
	}
}
