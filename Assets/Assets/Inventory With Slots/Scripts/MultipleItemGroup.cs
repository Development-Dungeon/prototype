using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Group", menuName ="New Item Group/Create Item Group")]
public class MultipleItemGroup : Item 
{

    [Header("Group Attributes")]
    public int quantity;
    public Item referenceItem;

}
