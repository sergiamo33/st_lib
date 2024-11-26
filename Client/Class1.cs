using CitizenFX.Core;
using CitizenFX.FiveM; // FiveM game related types (client only)
using CitizenFX.FiveM.Native; // FiveM natives (client only)
using System;
using System.Collections.Generic;


namespace Client
{
    public class Class1 : BaseScript
    {
        private bool displayText = false;
        private string text;
        private bool isDrawing = false;
        private float setDistance = 1;
        
        private readonly List<MarkerData> markers = new List<MarkerData>();
        public Class1()
        {
            EventHandlers["startDrawingText"] += new Action<string>(StartDrawingText);
            EventHandlers["stopDrawingText"] += new Action(StopDrawingText);
            EventHandlers["AddMarker"] += new Action<int, Vector3, Vector3, Vector3, Vector3, int, int, int, int, bool, bool, float>(AddMarker);
            EventHandlers["RemoveMarker"] += new Action<Vector3>(RemoveMarker);
            Tick += DrawText3dContinuously;
            Tick += DrawMarkersContinuously;
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
        
        private void CheckMarkerDistance()
        {
            var playerPed = Natives.PlayerPedId();
            var playerPos = Natives.GetEntityCoords(playerPed, true);

            foreach (var marker in markers)
            {
                var distance = Natives.GetDistanceBetweenCoords(playerPos.X, playerPos.Y, playerPos.Z, marker.Position.X, marker.Position.Y, marker.Position.Z, true);
                if (distance > setDistance)
                {
                    marker.IsActive = false;
                }
                else
                {
                    marker.IsActive = true;
                }
            }
        }
        private async Coroutine DrawMarkersContinuously()
        {
                CheckMarkerDistance();
                foreach (var marker in markers)
                {
                    if (marker.IsActive)
                    {
                        Natives.DrawMarker(marker.Type, marker.Position.X, marker.Position.Y, marker.Position.Z, marker.Direction.X, marker.Direction.Y, marker.Direction.Z, marker.Rotation.X, marker.Rotation.Y, marker.Rotation.Z, marker.Scale.X, marker.Scale.Y, marker.Scale.Z, marker.Color.Red, marker.Color.Green, marker.Color.Blue, marker.Color.Alpha, marker.BobUpAndDown, marker.FaceCamera, 2, false, null, null, false);
                    }
                }
        }
        private void AddMarker(int type, Vector3 position, Vector3 direction, Vector3 rotation, Vector3 scale, int red, int green, int blue, int alpha, bool bobUpAndDown, bool faceCamera, float distance)
        {
            var marker = new MarkerData
            {
                Type = type,
                Position = position,
                Direction = direction,
                Rotation = rotation,
                Scale = scale,
                Color = new MarkerColor(red, green, blue, alpha),
                BobUpAndDown = bobUpAndDown,
                FaceCamera = faceCamera,
                SetDistance = distance,
                IsActive = true
            };

            markers.Add(marker);
            setDistance = distance;
        }
        
        private void RemoveMarker(Vector3 position)
        {
            var marker = markers.Find(m => m.Position == position);
            if (marker != null)
            {
                marker.IsActive = false;
                markers.Remove(marker);
            }
        }
        
        private class MarkerData
        {
            public int Type { get; set; }
            public Vector3 Position { get; set; }
            public Vector3 Direction { get; set; }
            public Vector3 Rotation { get; set; }
            public Vector3 Scale { get; set; }
            public MarkerColor Color { get; set; }
            public bool BobUpAndDown { get; set; }
            public bool FaceCamera { get; set; }
            public bool IsActive { get; set; }
            public float SetDistance { get; set; }
        }

        private class MarkerColor
        {
            public int Red { get; }
            public int Green { get; }
            public int Blue { get; }
            public int Alpha { get; }

            public MarkerColor(int red, int green, int blue, int alpha)
            {
                Red = red;
                Green = green;
                Blue = blue;
                Alpha = alpha;
            }
        }
    }
}