using UnityEngine;
using System.Collections.Generic;

public class Workbench : MonoBehaviour
{
    public Transform spawnPoint;   // Onde o item final vai nascer
    public GameObject defaultResultPrefab; // Prefab do item padr�o (caso n�o haja combina��o)

    private List<string> components = new List<string>(); // Armazena os nomes dos componentes atuais

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
        if (components.Count == 0)
        {
            Debug.Log("Nenhum componente na Workbench!");
            return;
        }

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
        // Exemplo simples de combina��es:
        if (components.Contains("Wood") && components.Contains("Stone"))
        {
            Debug.Log("Criou um Machado!");
            return Resources.Load<GameObject>("Axe"); // precisa estar em Resources
        }
        else if (components.Contains("Stone") && components.Contains("Iron"))
        {
            Debug.Log("Criou uma Espada!");
            return Resources.Load<GameObject>("Sword");
        }

        // Se n�o bate com nenhuma receita, retorna o item padr�o
        return defaultResultPrefab;
    }
}
