using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPersonList", menuName = "CASUS/Person List")]
public class PersonList : ScriptableObject
{
    public List<PersonData> persons = new List<PersonData>();
}
