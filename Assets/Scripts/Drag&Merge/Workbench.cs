using UnityEngine;
using System.Collections.Generic;

public class Workbench : MonoBehaviour
{
    public Transform spawnPoint;   // Onde o item final vai nascer
    public GameObject defaultResultPrefab; // Prefab do item padr�o (caso n�o haja combina��o)

    public List<string> components = new List<string>(); // Lista utilizada para ordenar em ordem alfab�tica
    public List<string> combinacao = new List<string>() {"Objeto1Objeto2"};
    public GameObject[] resultados ;

    public string comps;

    // Quando um componente entra na �rea da Workbench
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string compName = other.gameObject.name.Replace("(Clone)", ""); 
            if (!components.Contains(compName))
            {
                components.Add(compName);
                Debug.Log($"Componente adicionado: {compName}");
            }
        }
    }

    // Quando um componente sai da �rea
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string compName = other.gameObject.name.Replace("(Clone)", "");
            if (components.Contains(compName))
            {
                components.Remove(compName);
                Debug.Log($"Componente removido: {compName}");
            }
        }
    }

    // M�todo chamado pelo bot�o
    public void Craft()
    {
        if (components.Count <= 1)
        {
            Debug.Log("Componentes faltando na Workbench!");
            return;
        }


        components.Sort();


        GameObject resultPrefab = GetResultFromComponents(); // escolhe o resultado
        Instantiate(resultPrefab, spawnPoint.position, Quaternion.identity);

        Debug.Log("Item criado!");

        // Aqui destru�mos os componentes usados (opcional)
        GameObject[] compsInWorkbench = GameObject.FindGameObjectsWithTag("Component");
        foreach (var c in compsInWorkbench)
        {
            if (c.GetComponent<Collider2D>().IsTouching(GetComponent<Collider2D>()))
                Destroy(c);
        }

        components.Clear(); // limpa a lista
    }

    // Decide o que criar dependendo dos componentes
    private GameObject GetResultFromComponents()
    {
        comps = components[0] + components[1];
        int indexResult = combinacao.IndexOf(comps);
        Debug.Log(indexResult);
        // Se n�o bate com nenhuma receita, retorna o item padr�o
        return resultados[indexResult + 1]; // Sempre index + 1 pq index 0 � o item faltando
    }
}
