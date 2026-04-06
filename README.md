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

```bash
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
