# TeamsMacroPad

Ein kleines 3-Button-Keypad zum Auslösen von Keyboard Shortcuts, z.B. für MS Teams.

![image](https://user-images.githubusercontent.com/14030572/125175359-d6020780-e1cb-11eb-9d02-76a15c737446.png)

![image](https://user-images.githubusercontent.com/14030572/125175536-29288a00-e1cd-11eb-9137-7594013ebf17.png)

Die Kommunikation läuft dabei über die serielle USB Verbindung vom Pad zu einer Companion Applikation auf dem PC, die die konfigurierbaren Hotkeys dann an das derzeitig aktive Fenster sendet.

## Hardware

- Arduino Nano
- 3 Push-Buttons
- 3D gedrucktes Gehäuse
- Mikro USB Kabel
- Kabel, Lötzinn ...

## Software

- Arduino Sketch
- C# Windows Applikation


## Anleitung
- die Box bauen (=die beiden STLs von [thingiverse drucken] (https://www.thingiverse.com/thing:4905351) oder was eigenes klöppeln)
- Buttons mit dem Arduino verbinden (Wichtig: Buttons an Ground und den jeweiligen Digital-Input-Port anschliessen und als INPUT_PULLUP definieren!)
- teams_mcropad_client.ino ggfs. anpassen und flashen (defaultmässig werden die Buttons an den digitalen Ports 2, 4 und 6 erwartet)
- Im Arduino Serial Monitor sollten beim Druck auf die Buttons jetzt die Strings TOGGLE_BUTTON_1/2/3 erscheinen
- um die nun auf Shortcuts zu mappen: Die Windows App (TeamsCompanionApp.7z) entpacken und neben der teams_companion.json irgendwo ablegen
- In der json ggs. den COM Port des Arduino Nanos ändern
- Applikation starten, MS Teams aufmachen und einem Meeting joinen, defaultmässig sind die Buttons mit den Shortcuts für "Kamera-Toggle", "Mikro-Toggle" und "Meeting-Verlassen" vorbelegt

## Komilieren
Das Microsoft Projekt ist eine C# Konsolenapplikation und benötigt 2 Dependencies: 
- System.IO.Ports (von Microsoft, zur Kommunikation via USB, über NuGet installieren)
- H.InputSimulator (von HavenDV basierend auf den anderen InputSimulator Projekten, simuliert Shortcuts, über NuGet installieren)

Andere Abhängigkeiten gibt es nicht, das Projekt sollte nun kompilieren. Eine einzelne fat-exe kann mittlerweile auch via Net Core erzeugt werden durch: 

    dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true

## Config
    "TOGGLE_BUTTON_1": {
      "shift": true,
      "control": true,
      "virtual_key": "VK_O"
      }
Bedeutet: Shortcut SHIFT+CTRL+O schicken. Die Liste der möglichen MS Teams Shortcuts lässt sich schwer verlinken, kann aber leicht gegoogelt werden.

Die Liste der gültigen Keycodes kann in der [InputSimulator](https://github.com/michaelnoonan/inputsimulator) Lib von Michael Noonan eingesehen werden, genauer [hier](https://github.com/michaelnoonan/inputsimulator/blob/master/WindowsInput/Native/VirtualKeyCode.cs).     
