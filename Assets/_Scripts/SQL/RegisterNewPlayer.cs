using UnityEngine;

public class RegisterNewPlayer : MonoBehaviour
{
    string _playerName;
    string _playerPass;

    public void SetPlayerName(string name)
    {
        _playerName = name;
    }

    public void SetPlayerPass(string pass)
    {
        _playerPass = pass;
    }

    public void CreateNewPlayer()
    {
        if (_playerName == null || _playerName.Length < 4)
        {
            Debug.Log("Nombre muy corto");
            return;
        }

        if (_playerPass == null || _playerPass.Length < 4)
        {
            Debug.Log("Contraseña muy corta");
            return;
        }
        ComandosSQLite.Instance.RegisterNewPlayer(_playerName, _playerPass);
    }
}
