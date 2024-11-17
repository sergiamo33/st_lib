using CitizenFX.Core;
using CitizenFX.FiveM; // FiveM game related types (client only)
using CitizenFX.FiveM.Native; // FiveM natives (client only)
using System;

namespace Client
{
    public class Class1 : BaseScript
    {
        private bool displayText = false;
        private string text;
        public Class1()
        {
            EventHandlers["startDrawingText"] += new Action<string>(StartDrawingText);
            EventHandlers["stopDrawingText"] += new Action(StopDrawingText);
            Tick += DrawText3dContinuously;
        }
        [EventHandler("startDrawingText", Binding.Local)]
        private void StartDrawingText(string newText)
        {
            text = newText;
            displayText = true;
        }

        [EventHandler("stopDrawingText", Binding.Local)]
        private void StopDrawingText()
        {
            displayText = false;
        }

        private async Coroutine DrawText3dContinuously()
        {
            if (displayText)
            {
                var playerPed = Natives.PlayerPedId();
                var pos = Natives.GetEntityCoords(playerPed, true);

                DrawText3D(pos, text);
            }
        }

        private void DrawText3D(Vector3 position, string text)
        {
            float worldX = position.X;
            float worldY = position.Y;
            float worldZ = position.Z + 1.0f;
            float screenX = 0.0f;
            float screenY = 0.0f;

            bool convertVectors = Natives.GetScreenCoordFromWorldCoord(worldX, worldY, worldZ, ref screenX, ref screenY);

            if (convertVectors)
            {
                Natives.SetTextScale(0.0f, 0.35f);
                Natives.SetTextFont(0);
                Natives.SetTextProportional(true);
                Natives.SetTextColour(255, 255, 255, 255);
                Natives.SetTextDropshadow(0, 0, 0, 0, 0);
                Natives.SetTextEdge(2, 0, 0, 0, 155);
                Natives.SetTextDropShadow();
                Natives.SetTextOutline();
                Natives.SetTextEntry("STRING");
                Natives.AddTextComponentString(text);
                Natives.DrawText(screenX, screenY);
            }
        }        
    }
}