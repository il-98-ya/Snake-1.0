﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class InputManager
    {
        //Load list of available Keyboard buttons
        private static Hashtable keyTable = new Hashtable();

        //Perform a check to see if a particular button is pressed.
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }
            return (bool)keyTable[key];
        }

        //Detect if a keyboard button is pressed
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
