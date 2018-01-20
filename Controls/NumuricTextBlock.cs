﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TeachingPlatformApp.Controls
{
    public class NumuricTextBlock : TextBox
    {
        public NumuricTextBlock()
        {
            this.PreviewKeyDown += NumuricTextBlock_PreviewKeyDown;
            InputMethod.SetIsInputMethodEnabled(this, false);
            DataObject.AddPastingHandler(this, (sen,e) => 
            {
                if (e.DataObject.GetDataPresent(typeof(String)))
                {
                    String text = (String)e.DataObject.GetData(typeof(String));
                    if (!IsNumberic(text))
                    {
                        e.CancelCommand();
                    }
                }
	            else
                {
                    e.CancelCommand();
                }
            }); 

        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            if(!IsNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
            base.OnTextInput(e);
        }

        private void NumuricTextBlock_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        public virtual bool IsNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))
                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

    }
}