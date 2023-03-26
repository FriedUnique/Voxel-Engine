using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core {
    public class GameTime {
        public float deltaTime { get; private set; }
        public float fixedDeltaTime { get; private set; }

        private float lastNormalTime;
        private float lastFixedTime;

        private Stopwatch stopWatch;

        public GameTime() {
            stopWatch = new Stopwatch();
            stopWatch.Start();
        }

        public float NormalTick() {
            float currentTime = stopWatch.ElapsedMilliseconds;

            deltaTime = (currentTime - lastNormalTime) / 1000.0f;
            lastNormalTime = currentTime;

            return deltaTime;
        }

        public float FixedTick() {
            float currentTime = stopWatch.ElapsedMilliseconds;

            fixedDeltaTime = (currentTime - lastFixedTime) / 1000.0f;
            lastFixedTime = currentTime;

            return fixedDeltaTime;
        }
    }
}
