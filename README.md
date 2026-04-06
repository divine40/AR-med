# AR-Med

AR-Med is an augmented reality (AR) educational application developed using Unity.  
The application is designed to help students understand human anatomy—specifically the heart and skeletal system—through interactive 3D visualization.

The system combines AR-based learning with interactive features such as touch labeling, simulation, and assessment through a quiz.

---

## Overview

Medical learning often relies on static diagrams that make it difficult to understand spatial relationships in anatomy. AR-Med addresses this challenge by allowing users to visualize and interact with anatomical structures in real time using augmented reality.

---

## Core Features

- User authentication (Firebase Authentication)
- AR-based heart visualization
- AR-based skeletal system visualization
- Interactive touch labeling of anatomical parts
- Arrhythmia simulation for the heart
- Skeleton interaction (explode view, fracture identification)
- Vein-Puncture Scene
- Quiz module for assessment
- Score tracking and feedback system

---

## Technologies Used

- Unity (Game Engine)
- Vuforia (AR tracking)
- Firebase Authentication (User login)
- C# (Scripting)
- TextMeshPro (UI rendering)
- Android SDK (Deployment)

---

## System Architecture (Summary)

The system consists of the following components:

- Mobile application (Unity-based)
- AR module using Vuforia for image tracking
- 3D models for heart and skeleton
- Firebase backend for authentication and data handling
- Local device storage for session data

---

## Requirements

### Software Requirements
- Unity (recommended version: [2022.3.60f1])
- Vuforia Engine
- Firebase SDK for Unity
- Android Build Support

### Hardware Requirements
- Android smartphone (mid-range or higher)
- Camera access enabled

---

## Installation and Setup

### Option 1: Run using APK

1. Download the APK file from the link below:
   - https://drive.google.com/file/d/12qQwuwrC3a4Lm2df-9J_7OeS8R7IMMqy/view?usp=sharing

2. Transfer the APK to your Android device.

3. Enable:
   - "Install unknown apps"
   - Camera permissions

4. Install the APK.

5. Launch the app and log in.

---

### Option 2: Run from Source Code (Recommended for Reviewers)

1. Clone the repository:
git clone https://github.com/divine40/AR-med.git
2. Open the project in Unity.
3. Install required dependencies:
Vuforia Engine
Firebase SDK
4. Configure Vuforia:
Enable Vuforia in Unity settings
Add your Vuforia license key
5. Configure Firebase:
Add Firebase project credentials
Enable Authentication
6. Build the project for Android:
File → Build Settings → Android → Build & Run

How to Use the Application
Open the application
Log in using credentials
Navigate to the main menu
Select the Heart AR module
Point the camera at the image target
Interact with the 3D heart (rotate, zoom, tap parts)
Explore the Skeleton module
Explore the 3d arm model 
Complete the quiz to assess understanding

Scene Flow

LoginScene - MenuScene - HeartScene - SkeletonScene - Veinpunturescene - QuizScene


Demo Video

https://drive.google.com/file/d/1lWaDURS46LbcFDZmlyKSVrvuAMMeRuJ1/view?usp=sharing

Testing

The application was tested using:

Unity Editor
Android device (Google Pixel 7 Pro)
Tested Features
AR image target detection
3D model rendering
Rotation and zoom interaction
Touch labeling accuracy
Arrhythmia simulation
Skeleton interaction modes
Quiz functionality and scoring

Performance Optimization

To ensure smooth performance on mid-range devices:

3D models were optimized (polygon reduction)
Textures were compressed
Scripts were optimized to reduce processing load
Lightweight UI components were used

Limitations
Limited number of test participants
No automated testing implemented
Performance may vary depending on device capability


Repository Link

https://github.com/divine40/AR-med

Author

Akunyiba Chimdalu Divinefavour
Capstone Project – AR-Med
