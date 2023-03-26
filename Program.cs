using GameEngine.Core;

namespace GameEngine {
    public class Program {
        public static void Main(string[] args) {
            Game game = new TestGame("", 800, 600);
            game.Run();
        }
    }
}