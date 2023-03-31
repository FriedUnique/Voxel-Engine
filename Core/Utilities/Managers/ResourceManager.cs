﻿using GameEngine.Core.Rendering;
using System.Collections.Generic;

namespace GameEngine.Core.Utilities.Managers {
    public sealed class ResourceManager {
        private static ResourceManager _instance = null;
        private static readonly object _loc = new();
        private IDictionary<string, TextureObject> _textureCache = new Dictionary<string, TextureObject>();

        //public const string defaultShaderPath = "Resources/Shaders/Default.shader";
        public const string defaultAtlasPath = "Resources/Textures/DefaultAtlas.png";

        public static ResourceManager Instance {
            get {
                lock (_loc) {
                    if (_instance == null) {
                        _instance = new ResourceManager();
                    }
                    return _instance;
                }
            }
        }

        public TextureObject LoadTexture(string textureName) {
            _textureCache.TryGetValue(textureName, out var value);
            if (value is not null) {
                return value;
            }
            value = TextureFactory.Load(textureName);
            _textureCache.Add(textureName, value);
            return value;
        }

    }
}