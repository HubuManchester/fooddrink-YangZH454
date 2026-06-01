# NutriSnap

## What is the aim of the app?
NutriSnap is a comprehensive, offline-first personal nutrition tracker designed to help users seamlessly log their dietary intake. The primary aim is to provide a highly accessible, hardware-rich mobile experience that allows users to record their meals, view detailed nutritional information, and stay mindful of their eating habits, whether they are online or completely offline.

## Author
* **Name:** Yang Zhiheng
* **Student ID:** 21906040/202331123002063

## Details on Features
NutriSnap integrates native Android hardware and advanced software architectural patterns to deliver a professional user experience:

* **Offline-First Data Syncing:** Utilizes a local SQLite database that seamlessly synchronizes with a cloud MockAPI. If the network drops, the app functions perfectly offline and caches data locally.
* **Native Hardware Integration:**
  * **Camera:** Capture and save photos of meals directly into the app.
  * **Geolocation:** Automatically fetch and record the exact coordinates of where a meal was eaten, with the ability to launch the native system Map app.
  * **Accelerometer (Shake):** Shake the device on the home screen to receive a random food recommendation.
  * **Text-to-Speech (TTS):** Accessible audio reading of food summaries with precise play/stop cancellation control.
  * **Haptic Feedback & Vibration:** Subtle haptics for UI button presses and strong vibrations for successful hardware actions (e.g., getting GPS).
* **UI/UX & Accessibility:**
  * Global Dark Mode / Light Mode support adapting to system preferences.
  * Dynamic "Large Text Mode" utilizing `Preferences` and `DynamicResource` for visually impaired users.
  * Professional `SwipeView` for intuitive record deletion.
  * Real-time search and filtering.

## Potential Future Features
If development were to continue, the following features would be implemented:
1. **Machine Learning Image Recognition:** Automatically identify the food and estimate calories using the device camera and a trained CoreML/TensorFlow Lite model.
2. **Barcode Scanner:** Use the camera to scan food packaging barcodes to automatically fetch nutritional data from public APIs.
3. **Social Sharing:** Allow users to share their healthy meal logs directly to social media platforms using native sharing intents.
4. **Data Visualization:** Integrate a charting library to show weekly calorie trends and macronutrient breakdowns.
