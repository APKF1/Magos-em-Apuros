using UnityEngine;
using System.Collections.Generic;

public class Workbench : MonoBehaviour
{
    public Transform spawnPoint;   // Onde o item final vai nascer
    public GameObject defaultResultPrefab; // Prefab do item padr�o (caso n�o haja combina��o)

    public List<string> components = new List<string>(); // Lista utilizada para ordenar em ordem alfab�tica
    public List<string> combinacao = new List<string>();
    public GameObject[] resultados ;

    public string comps;

    // Quando um componente entra na �rea da Workbench
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string AWOOOBA = other.gameObject.name;
            components.Add(AWOOOBA);
        }
    }

    // Quando um componente sai da �rea
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Component"))
        {
            string AWOOOBA = other.gameObject.name;
            components.Remove(AWOOOBA);
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
        for (int i = 0; i < 2; i++)
        {
            string compName = components[i];
            if (compName.Contains("(Clone)"))
            {
                compName =  compName.Replace("(Clone)", "");
                components[i] = compName;
            }
        }
        comps = components[0] + components[1];
        int indexResult = combinacao.IndexOf(comps);
        Debug.Log(indexResult);

        // Se n�o bate com nenhuma receita, retorna o item padr�o
        return resultados[indexResult + 1]; // Sempre index + 1 pq index 0 � o item faltando
    }
}
