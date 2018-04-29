# patientmatch

Microsoft Garage Jump Into AI Hackathon April 2018

## Goal
Prevent wrong patient medical errors through facial recognition

## Description
When the program starts, it streams video to the screen, and immediately begins face detection on frames of the video. If a face has been detected in one of the frames, we pass the face information to Cognitive Services to see if it matches an expected face for this room, as determined by an orchestration service.

## Tools and technologies used
* Raspberry Pi 3b
* Windows 10 IoT Core
* Microsoft LifeCam HD-3000
* Microsoft Cognitive Services Face API