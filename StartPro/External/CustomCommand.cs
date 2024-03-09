using System;
using System.Windows.Input;

namespace StartPro.External;

public class CustomCommand(Action executeMethod) : ICommand
{
    public event EventHandler CanExecuteChanged = delegate { };

    private readonly Func<bool> TargetCanExecuteMethod;

    bool ICommand.CanExecute(object o)
        => TargetCanExecuteMethod != null && TargetCanExecuteMethod( );

    public void RaiseCanExecuteChanged( )
        => CanExecuteChanged(this, EventArgs.Empty);

    void ICommand.Execute(object o)
        => executeMethod?.Invoke( );
}
