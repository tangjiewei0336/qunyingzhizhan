using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//扩展InspectorEditorTest类在Inspector面板的显示内容
[CustomEditor(typeof(CS_RouteSave))]
[CanEditMultipleObjects]
public class InspectorEditor : Editor
{
    //重写OnInspectorGUI类(刷新Inspector面板)
    public override void OnInspectorGUI()
    {
        //继承基类方法
        //base.OnInspectorGUI();

        var elements = this.serializedObject.FindProperty("RouteCollection");

        // 属性元素可见，控件展开状态
        if (EditorGUILayout.PropertyField(elements))
        {
            // 缩进一级
            EditorGUI.indentLevel++;
            // 设置元素个数
            elements.arraySize = EditorGUILayout.DelayedIntField("Size", elements.arraySize);
            // 绘制元素
            for (int i = 0, size = elements.arraySize; i < size; i++)
            {
                // 检索属性数组元素
                var element = elements.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element);
            }
            // 重置缩进
            EditorGUI.indentLevel--;
        }
        // 空格
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("路径操作", new GUIStyle("HeaderLabel"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("OperatingRoute"));

        //获取要执行方法的类
        CS_RouteSave targetScript = (CS_RouteSave)target;
        //绘制Button
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("插入路径"))
        {
            targetScript.InsertRoute();
        }
        if (GUILayout.Button("克隆路径"))
        {
            targetScript.CloneRoute();
        }
        if (GUILayout.Button("删除路径"))
        {
            targetScript.DeleteRoute();
        }
        GUILayout.EndHorizontal();
        this.serializedObject.ApplyModifiedProperties();

    }
}