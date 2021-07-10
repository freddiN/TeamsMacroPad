using System;
using System.IO;
using System.IO.Ports;
using System.Text.Json;
using WindowsInput;
using WindowsInput.Native;

// C:\Users\freddi\source\repos\TeamsCompanionApp>dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
namespace TeamsCompanionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--== Freddi's MS-Teams Companion App ==--");
            Console.WriteLine("a config named \"teams_companion.json\" is expeced to exist next to this program");

            Console.WriteLine("The following serial ports were found:");
            foreach (string port in SerialPort.GetPortNames()) {
                Console.WriteLine(port);
            }

            JsonElement root = JsonDocument.Parse(File.ReadAllBytes("teams_companion.json")).RootElement;
            Console.WriteLine("\r\nConfig:");
            Console.WriteLine(root);
           
            SerialPort ComPort = new SerialPort{
                PortName = root.GetProperty("serial").GetProperty("port").GetString(),
                BaudRate = root.GetProperty("serial").GetProperty("baudrate").GetInt32(),
                Parity = Parity.None,          // Parity bits = none  
                DataBits = 8,                  // No of Data bits = 8
                StopBits = StopBits.One        // No of Stop bits = 1
            };

            try {
                ComPort.Open();

                while (true) {
                    string serialString = ComPort.ReadLine().Trim();
                    Console.WriteLine("received: >" + serialString + "<");

                    if (string.Equals(serialString, "TOGGLE_BUTTON_1", StringComparison.OrdinalIgnoreCase)) {
                        sendToggle(root.GetProperty("mapping").GetProperty("TOGGLE_BUTTON_1"));
                    } else if (string.Equals(serialString, "TOGGLE_BUTTON_2", StringComparison.OrdinalIgnoreCase)) {
                        sendToggle(root.GetProperty("mapping").GetProperty("TOGGLE_BUTTON_2"));
                    } else if (string.Equals(serialString, "TOGGLE_BUTTON_3", StringComparison.OrdinalIgnoreCase)) {
                        sendToggle(root.GetProperty("mapping").GetProperty("TOGGLE_BUTTON_3"));
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            } finally {
                ComPort.Close();
            }

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        static void sendToggle(JsonElement mapping) {
            /*"TOGGLE_BUTTON_1": {
                "shift": true,
               "control": true,
               "virtual_key": "VK_O"
            }*/

            InputSimulator i = new InputSimulator();
            if (mapping.GetProperty("shift").GetBoolean()) {
                i.Keyboard.KeyDown(VirtualKeyCode.SHIFT);
            }
            if (mapping.GetProperty("control").GetBoolean()) {
                i.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            }

            i.Keyboard.KeyPress((VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), mapping.GetProperty("virtual_key").GetString()));

            if (mapping.GetProperty("shift").GetBoolean()) {
                i.Keyboard.KeyUp(VirtualKeyCode.SHIFT);
            }
            if (mapping.GetProperty("control").GetBoolean()) {
                i.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            }

            Console.WriteLine(" -> toggle sent");
        }
    }
}