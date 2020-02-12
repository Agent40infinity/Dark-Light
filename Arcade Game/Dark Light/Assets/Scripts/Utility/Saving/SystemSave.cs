﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


/*---------------------------------/
 * Script by Aiden Nathan.
 *---------------------------------*/

public static class SystemSave
{
    public static void SavePlayer(Player player) //Creates reference to player and allows the player to save their data to a save file.
    {
        BinaryFormatter formatter = new BinaryFormatter(); //Creates a new BinaryFormatter to allow for data conversion.
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player); //Creates reference to the PlayerData and allows it to be called upon.

        formatter.Serialize(stream, data); //Serializes all data being transfered from PlayerData.
        stream.Close();
    }

    public static PlayerData LoadPlayer(Player player) //Allows the player to load their data from the system.
    {
        string path = Application.persistentDataPath + "/save.dat"; //Creates a check for the directory of the file.
        if (File.Exists(path)) //Checks if the path and file exist.
        {
            BinaryFormatter formatter = new BinaryFormatter(); //Creates a new reference to the Binary Formatter.
            FileStream stream = new FileStream(path, FileMode.Open); //Creates a reference to allow for the file to open.

            PlayerData data = formatter.Deserialize(stream) as PlayerData; //Loads the data from the file to the PlayerData script.

            player.dashUnlocked = data.dashUnlocked;
            GameObject[] lampControllers = GameObject.FindGameObjectsWithTag("Save");
            for (int i = 0; i < data.lampsLit.Length; i++)
            {
                if (data.lampsLit[i] && lampControllers[i].GetComponent<LampController>())
                {
                    lampControllers[i].GetComponent<LampController>().LoadLamp();
                }
            }
            Lamp.lastSaved = data.lampIndex;
            Vector3 playerPos = new Vector3(Lamp.lPos[Lamp.lastSaved].position.x, Lamp.lPos[Lamp.lastSaved].position.y, 0);
            player.transform.position = playerPos;

            stream.Close(); //Closes the file.
            return data; //Returns the data within "data".
        }
        else
        {
            Debug.LogError("Save file not found in " + path); //Debug log to tell us that the directory is missing:=.
            return null;
        }
    }
}
