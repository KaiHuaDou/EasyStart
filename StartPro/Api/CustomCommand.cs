using System;
using System.Windows.Input;

namespace StartPro.Api;

public class CustomCommand(Action executeMethod) : ICommand
{
    public event EventHandler CanExecuteChanged = delegate { };

    bool ICommand.CanExecute(object o)
        => true;

    public void RaiseCanExecuteChanged( )
        => CanExecuteChanged(this, EventArgs.Empty);

    void ICommand.Execute(object parameter)
        => executeMethod?.Invoke( );
}
