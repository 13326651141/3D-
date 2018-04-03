# 1.1.游戏对象运动的本质是什么？
游戏对象运动的本质，其实是游戏对象跟随每一帧的变化，空间地变化。这里的空间变化包括了游戏对象的transform属性中的position跟rotation两个属性。
一个是绝对或者相对位置的改变，一个是所处位置的角度的旋转变化。

# 1.2.三种方法实现物体的抛物线运动

## 1.2.1.用transform实现
实现的代码如下所示：
``` C#
public float speed = 1f;
void Update () {
    this.transform.position += speed * Vector3.up * Time.deltaTime ;
    this.transform.position += 10 * Vector3.right * Time.deltaTime ;
    speed+= 0.1f;
}
```
## 1.2.2.用vector3实现
实现代码如下所示：
``` C#
public float speed = 1f;
void Update () {
    Vector3 tmp = new Vector3(10 * Time.deltaTime, speed * Time.deltaTime, 0);
    this.transform.position += tmp;
    speed+= 0.1f;
}

```
该代码的效果和1.2.1的效果图是一样的，只是实现的方法不一样。

## 1.2.3.把+= vector3 变成 transform.Translate(vector3);
首先创建两个对象，Target1和Target2，两个对象的位置放置在很远的地方，然后让物体朝着两个目标个体移动，即可得到抛物线的运动轨迹。
``` C#
public float speed = 1f;
void Update () {
    Vector3 tmp = new Vector3(10 * Time.deltaTime, speed * Time.deltaTime, 0);
    this.transform.Translate(tmp);
    speed+= 0.1f;
}
```
该代码的实现效果和1.2.1的效果一样

# 1.3.实现一个完整的太阳系
第三个步骤是通过实现一个太阳系来熟悉游戏对象运动的操作

首先我们先制作好样式，就是八大行星的壁纸贴图等等，相关的素材可以在网上找到，直接将其贴到相应创造出来的球体即可以



然后我们开始写公转和自转的脚本，因为八大行星都是绕太阳公转，而月球是绕地球公转的，所有八大行星都以太阳为中心点旋转，而月球则以地球为中心点旋转。

代码如下所示：
``` C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plantMove : MonoBehaviour
{

    // Use this for initialization  
    void Start()
    {
   
    }

    // Update is called once per frame  
    void Update()
    {

        GameObject.Find("Sun").transform.Rotate(Vector3.up * Time.deltaTime * 5 );

        GameObject.Find("Mercury").transform.RotateAround(Vector3.zero, new Vector3(0.1f, 1, 0), 60 * Time.deltaTime);
        //设置公转的方向和速度  方向轴为（0， 1， 0） 速度为 60
        GameObject.Find("Mercury").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 58);
        //设置自转 自转速度为10000/58   58是水星的自传周期  倒数就是时间  下同

        GameObject.Find("Venus").transform.RotateAround(Vector3.zero, new Vector3(0, 1, -0.1f), 55 * Time.deltaTime);
        GameObject.Find("Venus").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 243);

        GameObject.Find("Earth").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 50 * Time.deltaTime);
        GameObject.Find("Earth").transform.Rotate(Vector3.up * Time.deltaTime * 10000);

         GameObject.Find("Moon").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), 5 * Time.deltaTime);
        GameObject.Find("Moon").transform.Rotate(Vector3.up * Time.deltaTime * 10000/27);

        GameObject.Find("Mars").transform.RotateAround(Vector3.zero, new Vector3(0.2f, 1, 0), 45 * Time.deltaTime);
        GameObject.Find("Mars").transform.Rotate(Vector3.up * Time.deltaTime * 10000);

        GameObject.Find("Jupiter").transform.RotateAround(Vector3.zero, new Vector3(-0.1f, 2, 0), 35 * Time.deltaTime);
        GameObject.Find("Jupiter").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.3f);

        GameObject.Find("Saturn").transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0.2f), 20 * Time.deltaTime);
        GameObject.Find("Saturn").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.4f);

        GameObject.Find("Uranus").transform.RotateAround(Vector3.zero, new Vector3(0, 2, 0.1f), 15 * Time.deltaTime);
        GameObject.Find("Uranus").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.6f);

        GameObject.Find("Neptune").transform.RotateAround(Vector3.zero, new Vector3(-0.1f, 1, -0.1f), 10 * Time.deltaTime);
        GameObject.Find("Neptune").transform.Rotate(Vector3.up * Time.deltaTime * 10000 / 0.7f);

    }
}
```

我们通过GameObject.Find("value")来找到各个球体 然后通过调用RotateAround（）设置公转，通过调用Rotate（）方法设置自转