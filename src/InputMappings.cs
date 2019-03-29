using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Keyboard = SadConsole.Input.Keyboard;

namespace flxkbr.unknownasofyet
{
    public static class InputMappings
    {
        public enum InputAction { Up, Down, Left, Right, Interact, Escape, Return, None };

        public static readonly Dictionary<InputAction, Keys[]> mappings = new Dictionary<InputAction, Keys[]>()
        {
            {InputAction.Up, new Keys[]{Keys.Up, Keys.W}},
            {InputAction.Down, new Keys[]{Keys.Down, Keys.S}},
            {InputAction.Left, new Keys[]{Keys.Left, Keys.A}},
            {InputAction.Right, new Keys[]{Keys.Right, Keys.D}},

            {InputAction.Interact, new Keys[]{Keys.E, Keys.Space}},
            {InputAction.Escape, new Keys[]{Keys.Escape}},
            {InputAction.Return, new Keys[]{Keys.Back}},
        };

        public static bool IsActionPressed(Keyboard info,  InputAction action)
        {
            if (!mappings.ContainsKey(action))
            {
                Console.Error.WriteLine($"InputMappings.IsActionPressed: No mapping found for InputAction {action}");
            }
            foreach (var key in mappings[action])
            {
                if (info.IsKeyPressed(key)) return true;
            }
            return false;
        }

        public static InputAction MovementPressed(Keyboard info)
        {
            if (IsActionPressed(info, InputAction.Left))
            {
                return InputAction.Left;
            }
            else if (IsActionPressed(info, InputAction.Up))
            {
                return InputAction.Up;
            }
            else if (IsActionPressed(info, InputAction.Right))
            {
                return InputAction.Right;
            }
            else if (IsActionPressed(info, InputAction.Down))
            {
                return InputAction.Down;
            }
            return InputAction.None;
        }
    }
}