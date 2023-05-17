using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public interface IController
    {

    }

    public abstract class Controller : MonoBehaviour, IController
    {

    }

    public abstract class UIController : Controller
    {

    }
}