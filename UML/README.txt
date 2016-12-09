Improvements:

- The CommandFactory has been refactored in order to remove 
  its enormous switch statement. It now uses a template method 
  to create ICommand objects. The template method is called 
  FactoryMode. The various FactoryMode is extended by multiple
  strategies (one for each type of ICommand). Each mode strategy 
  creates a single type of ICommand. This allows the CommandFactory
  to choose between multiple factory modes at run time without a
  massive switch statement. This makes the code more readable
  and more extensible. Additionally, it allows for CommandParameter
  object validation to be localized.

- The way the program renders DrawableObjects now figures out which
  objects have changed and re-renders only those objects (rather
  than re-rendering all of them each time one changes). This allows
  the program to run much faster when the user has added a large 
  number ofW objects to his/her drawing.

- There is now a MacroCommand that allows multiple ICommands
  to be executed and undone at once. This allows the undo
  functionality of the program to work more smoothly (for instance,
  duplication is now undone in one step rather than three).

- The user can now move objects by dragging them with the mouse.
  This means that all consecutive move commands will be undone at
  the same time (even if the user leaves an object at a point for
  a little while before moving it some more). Overall, this should
  make moving objects around more intuitive for the user.

- The unit tests have been updated in order to test these new features.

- The UML class diagram has been split into three smaller diagrams. Additionally,
  there is one new sequence diagram. The last three diagrams in UML.pdf are from the
  previous submission and are included for convenience. The UML from the previous
  submission is include in the Old UML folder.