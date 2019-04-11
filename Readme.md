# Cartographer
Cartographer is a lightweight, asynchronous, .Net logging library.

# Features
Cartographer is a logging library designed to log messages asynchronously so that it impacts the speed of your code as little as possible.
  - Easily log at any time with context data such as caller class, method, line number and thread ID.
  - Log exception stack traces.
  - Use logging levels and apply filters to set the logging level.
  - Print to the console.
  - Use the experimental stack trace functionality to see how your code gets compiled in release mode.

# Usage example:
Supply a logging location file path including the file extension 

    var cartographer = new Cartographer.Cartographer("log_file_location");
    cartographer.Log("Your log message", LoggingLevel.Info);
    
Caught an exception?
    
    cartographer.Log("Caught an exception", LoggingLevel.Error, exception);

You do not need to supply the optional arguments. Cartographer uses callerMember attributes to get the class name, method name and line numbers.

License
----

MIT
