using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SerializedPoint : IEquatable<SerializedPoint>
{
    public float x;
    public float y;

	public int ix
	{
		get
		{
			return (int)x;
		}//get
	}//ix

	public int iy
	{
		get
		{
			return (int)y;
		}//get
	}//iy

	//------------------------
    //CONSTRUCTORS
    //------------------------
    public SerializedPoint(float x, float y)
    {
        this.x = x;
        this.y = y;
    }//constructor
    
    public SerializedPoint(int x, int y)
    {
        this.x = (float)x;
        this.y = (float)y;
    }//constructor
    
    public SerializedPoint(SerializedPoint point)
    {
        this.x = (float)point.x;
        this.y = (float)point.y;
    }//constructor
    
    public SerializedPoint(Vector2 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
    }//constructor
    
    public SerializedPoint(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
    }//constructor

    //------------------------
    //EQUATING
    //------------------------
    public override bool Equals (object obj)
    {
        
        if(obj == null)
            return false;
        
        SerializedPoint other = (SerializedPoint)obj;
        if(other == null)
            return false;
        
        return other.x == this.x && other.y == this.y;
    }

    public bool Equals (SerializedPoint other)
    {
        if(other == null)
            return false;
        
        return other.x == this.x && other.y == this.y;
    }//Equals
    
    public override int GetHashCode()        
    {            
        return (int)x ^ (int)y;        
    }  

    //------------------------
    //OPERATORS
    //------------------------
    static public implicit operator SerializedPoint(Vector2 vector)
    {
        return new SerializedPoint(vector);
    }//static
    
    static public implicit operator SerializedPoint(Vector3 vector)
    {
        return new SerializedPoint(vector);
    }//static
    
    static public implicit operator Vector2(SerializedPoint point)
    {
        return new Vector2(point.x, point.y);
    }//static
    
    static public implicit operator Vector3(SerializedPoint point)
    {
        return new Vector3(point.x, point.y);
    }//static

	static public SerializedPoint operator  *(SerializedPoint point1,SerializedPoint point2)
	{
		return new SerializedPoint(point1.x * point2.x, point1.y * point2.y);
	}//*

	static public SerializedPoint operator  *(SerializedPoint point1,int scalar)
	{
		return new SerializedPoint(point1.x * scalar, point1.y * scalar);
	}//*

	static public SerializedPoint operator  +(SerializedPoint point1,SerializedPoint point2)
	{
		return new SerializedPoint(point1.x + point2.x, point1.y + point2.y);
	}//*
    
    //------------------------
    //TOSTRING
    //------------------------
    public override string ToString()
    {
        return "[" + x + "," + y + "]";
    }//ToString
}//SerializedPoint