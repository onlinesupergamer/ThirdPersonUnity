/*This pulls the saved data from the Persistant Save File
 * ALWAYS PULL/CHANGE REFERENCES FROM THIS FILE, NOT THE PERSISTANT DATA
 * 
 * 
 * 
 * 
 * 
 */



public static class CurrentPlayerData
{
    public static float health;

    


    public static void GetPlayerData() 
    {
        health = PersistantPlayerData.health;
        
    }
}
