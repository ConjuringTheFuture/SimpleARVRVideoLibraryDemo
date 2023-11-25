# SimpleARVRVideoLibraryDemo
# Preview
https://github.com/ConjuringTheFuture/SimpleARVRVideoLibraryDemo/assets/115386259/0dcd59ad-e027-4a1d-829f-1ad541a4e47f

## Description
I was tasked with spending a few hours quickly prototyping a simple demo of an ARVR video library browser. Included in this repo is the Unity project source of my quick attempt. 
The project uses OpenXR as the ARVR interface layer. I've built and tested it as both a PC VR application as well as a native Quest 2 application. 
It uses OpenXR, specificly the XR Hands and XR Interaction Toolkit packages as provided from Unity to prepare for the VisionPro.

It should directly build for the VisionPro with minimal changes as outlined here:
https://discussions.unity.com/t/welcome-to-unitys-visionos-beta-program/270282?elqTrackId=6b37b193586b45efa9407bc8fac428e3&elqaid=4647&elqat=2

### Usage and inspiration
I was inspired by the AR phones you see in the Amazon show Upload.  
![inspiration](https://github.com/ConjuringTheFuture/SimpleARVRVideoLibraryDemo/assets/115386259/bae4ed8f-a9a6-4b96-bd58-7ebe8db003e5)

I like how they place the UI in the nook of the user's hand when they make an L shape with their thumb and wanted to see if I could mock up some similar behavior. So this demo attempts to work similarly. 

When the user makes a closed fist L shape with their left hand a video browser UI appears and they are able to browse through some video thumbnail and select videos to view with their right hand.    

### Limitations
All assets are placeholder and baked into the application. While you can browse and playback videos, the thumbnails and videos themselves are not loaded live from anywhere and just placeholder assets are displayed. 
You can't playback multiple videos nor is their really any logic differentiating between which thumbnail was selected. The pose detection works fairly well but is a bit brute force and should totally be improved for a production setting. There are a ton of refinements that need to be done to really make this feel good but I was surprised how refined the XR interaction toolkit already is. This demo simply servers to prove out the concept of the interaction, and for that I think it works. Definitely has a lot of room to improve though.   
