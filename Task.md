# Windows Forms - zadanie 1 

## Windows Forms - part 1

### Translator

*  #### **✓** Main window:
    *  **✓** Starts in the center of the screen 
    *  **✓** The minimum window size: 500x400
    *  **✓** When the window is resized, it behaves like in the example app: textBoxes change their sizes, "Translate!" button changes location, the panel on the left side of the window keeps its width.
    *  **✓** Text boxes: font Calibri, size 12pt.
    *  **✓** Menu: "Load" and "Export" buttons.
* ####  **✓** File upload:
    *  **✓** The "Load" button launches the file selection window that filters txt files. After loading the file, the words are displayed in two columns in the panel on the left. The column headers should be loaded from the first line of the file. Loading the next file overwrites the contents of the panel.
    *  **✓** Only words that consist entirely of letters can be added to the dictionary (they can not contain numbers or punctuation marks), empty word cannot be added. If such words are present in the file being read, they should be omitted.
    *  **✓** Add the "Export" option, which allows you to save the dictionary to a .txt file.
* #### **✓** Translator:
    *   **✓** The upper text box is used to enter text, which we want to translate.
    *  **✓** When the "Translate!" button is pressed, text from the upper text box is translated into the bottom text box.
    *  **✓** If the word is not in the dictionary, the word from the upper box is rewritten to the bottom text box, but the word font color changes to red
    *  **✓** Layout (spaces, new lines) from the upper text box should be preserved.
    *  **✓** Words should be translated regardless of whether they are written in upper or lower case letters.
* ####  Hints:
    *   RichTextBox,Dock, Anchor
    *   ListView, OpenFileDialog
    *   StringComparer.CurrentCultureIgnoreCase
*   _Note: In all doubtful and untold aspects application should behave like example app (except possible bugs)_

* ####  Scoring:
    *   "Main window" and "File upload" sections: 4 points
    *   "Translator" section: 4 points
    *   _Note: It is not possible to obtain points for incomplete functionality_

## Windows Forms - part 2

* #### **✓** Dictionary panel:
    *  **✓** It does not change width when whole window is resized. It is possible to change its width by using grey splitter.
    *  **✓** It is possible to remove content from dictionary panel with delete button. It is possible to remove many elements at once.
    *  **✓** "Add new word" button enables adding a new translation.
    *  **✓** It is possible to sort dictionary in ascending order by clicking on the column's header.Pressing the same column again causes descending sorting.
* #### **✓** "Add new word" window:
    *  **✓** It appears in the middle of the parent window.
    *  **✓** It is not posible to resize, maximize or minimize
    *  **✓** It allows to add translations in the currently selected languages.
    *  **✓** When the "Add new word" window is visible, the main window is inactive.
    *  **✓** When the numbers, punctuation marks or just white characters are entered, the validation icon appears. After hovering over the cursor, information about the reason for validation appears.
    *  **✓** After pressing the "OK" button, the window closes and a new translation is added to the dictionary. If at the moment of pressing "OK", in some textbox there is an incorrect value, an error window appears.
    *  **✓** The "Cancel" button closes the window and nothing is added to the dictionary
    *  **✓** If you press PPM on the untranslated word in the bottom text panel, a context menu will appear, with the option to add this word to the dictionary. After selecting this option, the "Add new word" window will appear with automatically completed word.
*  #### **✓** Font panel:
    *  **✓** It is possible to add bold, italic and underline text in the upper text panel. You can also change the style and color of the font as well as the background color.
    *  **✓** In the application, the icons should be loaded from Resources.
    *  **✓** Enabled font styles (bold, italic or underline) are marked by a button border.
    *  **✓** Styles can be combined with each other (i.e. it is possible for the text to be bolded, in italics and underlined at once).
    *  **✓** After pressing the buttons responsible for changing the background color or the font color, the color selection dialog box appears.
    *  **✓** It is possible to move the font panel under the upper text field.
* #### **✓** Drag&Drop:
    *  **✓** It is possible to add words to the list by dragging and dropping the txt file to the list.
*  #### Hints:
    *   ContextMenuStrip
    *   ToolStripContainer, ToolStrip
    *   SplitContainer
    *   ErrorProvider
    *   DialogResult
    *   IComparer
*   _Note: In all doubtful and untold aspects application should behave like example app (except possible bugs)_

*  #### Approximate scoring:
    *   deleting elements from the dictionary: 1 point
    *   column sorting: 2 points
    *   "Add new word" window (no validation): 1 point
    *   validation of the "Add new word" window: 2 points
    *   context menu and correctly completed "Add new word" window: 1 point
    *   fonts panel (no moving): 2 points
    *   Moving the font change panel: 2 points
    *   Drag&Drop: 1 point
    *   _Note: To pass part 2, all of the functionality of the part 1 must be fulfilled._