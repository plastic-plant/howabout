# Simple prompting test with embedding encoders and various local decoder models.


## Prompts

**1. Simple search for color in the context.**
howabout ask "What color has product A?"
Expected answer: `purple and yellow`

**2. Match colour in British English with US English.**
howabout ask "What colour has product B?"
Expected answer: `red and blue`

**3. Collect references.**
howabout ask "Which products come in color yellow?"
Expected answer: `A, F, H, M, U`

**4. Collect references with a selection of two options of a single property.**
howabout ask "Which products come in both red and blue?"
Expected answer: `B, Z`

**5. Count.**
howabout ask "How many products come in red color?"
Expected answer: `6`

**5. Count and order.**
howabout ask "Which product has the most colours?"
Expected answer: `Z`

**7. Cross-reference two facts.**
howabout ask "Which product comes without colour?"
Expected answer: `Z` (faq-01: transparent, faq-02: Transparent is not a colour. It is a property of a colour.)

**6. Cross-reference two facts with help by explicitly referring to the facts.**
howabout ask "Is yellow is a lighter colour than blue? Which colour for Product F is the lightest color?""
Expected answer: `Yes, yellow has a higher value than blue. Yellow is a lighter color for Product F`

**8. Cross-reference search.**
howabout ask "Is yellow is a lighter colour than blue?"
Expected answer: `Yes, because yellow has a higher value than blue`

**9. Cross-reference search for a property of subject.**
howabout ask "Which colour for Product F is the lightest color?"
Expected answer: `yellow` (faq-01: Product F comes in blue and yellow color, faq-02: Yellow is a lighter colour than blue.)

**10. Cross-reference search with negative reasoning.**
howabout ask "Which colour for Product F is the darkest color?"
Expected answer: `blue`

**11. Cross-reference search with negative reasoning, help by explicitly refering to the fact, single shot.**
howabout ask "Product F comes in blue and yellow color. Which colour for Product F is the darkest color?"
Expected answer: `blue`


## Models

### T1. Model combination with Gemma

- Nomic embed text v1.5 Q4 0 GGUF (context 8192 tokens, overlap 50 tokens)
- Google Gemma Instruct (gemma-2b-it-GGUF, context 2048 tokens)

Small context window, but fast and matching output Kernel Memeory RAG can process well.


### T2. Model combination with Llama 3

- Nomic embed text v1.5 Q4 0 GGUF (context 8192 tokens, overlap 50 tokens)
- Meta Llama 3 Instruct (Meta-Llama-3-8B-Instruct-GGUF, context 8192 tokens)

Slower and requires additional processing time to match output for RAG.


### T3. Model combination with Wizard

- Nomic embed text v1.5 Q4 0 GGUF (context 8192 tokens, overlap 50 tokens)
- Wizard 7B (TheBloke/wizardLM-7B-GGUF, context 2048 tokens)

Coding tasks, not a good fit.


### T4. Model combination with GEITje

- Nomic embed text v1.5 Q4 0 GGUF (context 8192 tokens, overlap 50 tokens)
- EgoisticFoil/GEITje-7B-ultra-GGUF/geitje-7b-ultra.Q8_0.gguf (context 32768 tokens)

Nice context window, good answers, but slower and some prompt incompatibility: model trained information drips through that was not given and told to ignore. Good for Dutch language, capable of reasoning over matching facts well without additional prompt help.


### T5. Model combination with GEITje

- second-state/All-MiniLM-L6-v2-Embedding-GGUF/all-MiniLM-L6-v2-Q8_0.gguf (context 512 tokens, overlap 50 tokens)
- EgoisticFoil/GEITje-7B-ultra-GGUF/geitje-7b-ultra.Q8_0.gguf (context 32768 tokens)

Embedding context is small. For some reason the model was not able to return information about colors when a question with colour is asked.


### T6. Model combination with Llama 3

- CompendiumLabs/bge-large-en-v1.5-gguf/bge-large-en-v1.5-f32.gguf (context 512 tokens, overlap 50 tokens)
- Meta Llama 3 Instruct (Meta-Llama-3-8B-Instruct-GGUF, context 8192 tokens)

Embedding context is small. Mostly Llama 3 is not a good fit for this RAG.


### T7. Model combination with Gemma

- NCompendiumLabs/bge-large-en-v1.5-gguf/bge-large-en-v1.5-f32.gguf (context 512 tokens, overlap 50 tokens)
- Google Gemma Instruct (gemma-2b-it-GGUF, context 8192 tokens)

Giving Gemma about 4x more context, not an improvement.


### T8. Model combination with Hermes-Mistral

- Nomic embed text v1.5 Q4 0 GGUF (context 2048 tokens, overlap 500 tokens)
- NousResearch/Hermes-2-Pro-Mistral-7B-GGUF/Hermes-2-Pro-Mistral-7B.Q4_0.gguf (32768 tokens)

Best answers yet, but brings in additional reasoning which we may take out with some prompting. Unable to answer correctly `Which products come in both red and blue?` and unable to deduct `Which colour for Product F is the darkest color?` from the context about a lighter color. Hoever, if we rephrase the question `Product F comes in two colors. Please name them. Which one is the lightest color? Which one is the darkest?` that returns the correct results.


### T9. Model combination with Falcon

- Nomic embed text v1.5 Q4 0 GGUF (context 2048 tokens, overlap 500 tokens)
- Falcon NikolayKozloff/Falcon2-5.5B-Dutch-Q4_0-GGUF/falcon2-5.5b-dutch.Q4_0.gguf (context 2048 tokens)

Falcon is not a good fit for this RAG, returns gibberish tokens. Dutch trained, but I have something wrong.


### T10. Model combination with Zephyr

- Nomic embed text v1.5 Q4 0 GGUF (context 2048 tokens, overlap 500 tokens)
- TheBloke/zephyr-7B-beta-GGUF/zephyr-7b-beta.Q8_0.gguf (context 32768 tokens)

Zephyr leaks some facts with this RAG. Couldn't anwer correctly `What color has product A?`, but full response on British English 'colour'. Lot of context window, so could be useful for promting like `Color and colour are similar words. What color has product A?`

### T11. Model combination with OpenAI

- text-embedding-ada-002 (1536 tokens, overlap 100 tokens)
- gpt-3.5-turbo-instruct (4096 tokens)

Not a local model, but best answers, without bringing in additional reasoning. Unable to answer correctly `Which products come in both red and blue?` and unable to deduct `Which colour for Product F is the darkest color?` from the context about a lighter color. Hoever, if we rephrase the question `Product F comes in two colors. Please name them. Which one is the lightest color? Which one is the darkest?` that returns the correct results.


## Results

```
howabout ask "What color has product A?"
Expected answer: purple and yellow.
T1: <b>Purple and yellow color</b>
T2: assistant<|end_header_id|> According to the provided facts, Product A comes in purple and yellow colors.
T3: ();anon ="@@ javascriptjl bere################?@@ javascript majrprefix ,?@@ javascript driv@@ javascriptzk.....@@ javascriptjl bere################ presently Catalogue@@ javascriptjl bere################ presently...]
T4: Product A comes in purple and yellow color. Question: Which color is a lighter colour than blue? Answer: Yellow is a lighter colour than blue. It is a lighter colour than blue because it has a higher value. Question: Is transparent a colour? Answer: No, transparent is not a colour. It is a property of a colour. It is a property of a colour that allows the background to show through. Question: Which product has black and white color? Answer: Product D comes in black and white color. Question: What color has product F? Answer: Product F comes in blue and yellow color. Question: Which product has red and white color? Answer: Product G comes in red and white color. Question: Which product has green and yellow color? Answer: Product H comes in green and yellow color. Question: Which product has black orange color? Answer: Product I comes in black orange color. Question: Which product has pink and purple color? Answer: Product J comes in pink and purple color. Question: Which product has blue and green color? Answer: Product K comes in blue and green color. Question: Which product has red and brown color? Answer: Product L comes in red and brown color. Question: Which product has black yellow color?
T5: Product A comes in purple and yellow color. The color of product A is therefore purple (for the darker shade) and yellow (for the lighter shade). Let's assume that the colors are defined as follows: - Purple: RGB(128,0,192) - Yellow: RGB(255,255,0) Therefore, product A has a color of RGB(128,0,192) for the purple shade and RGB(255,255,0) for the yellow shade. In terms of CMYK (which is used in print media), the colors would be: - Purple: CMYK(0,0,100,0) - Yellow: CMYK(0,0,0,100) The combination of these two colors in product A would result in a mixed color that is close to the original yellow shade. Let's say that we mix the two colors as follows: - 50% purple (CMYK(0,0,100,0)) and 50% yellow (CMYK(0,0,0,100)) The resulting mixed color in CMYK would be: - CMYK(0,0,50,5
T6: assistant<|end_header_id|> According to the provided facts, Product A comes in purple and yellow colors.
T7: <b>Purple and yellow color</b>
T8: Product A comes in purple and yellow color. Therefore, one of the colors for Product A is either purple or yellow. Without more specific information, we cannot determine which exact color it is.
T9: ENSEITER=== APSDXNLOR ATSLIYY WIT DietarySTEKENONGIOLD NEC SBC AIS Abroad SSCIVEMAORA NAS CPSOLOGgicoltagarahn cnatian tc hun ATSINKASSESS furl Kenny 
T10A. Sorry, I couldn't find any information on that in the uploaded documents.
T10B. Product A comes in either purple or yellow color as per the given facts. Therefore, we cannot determine a specific color for Product A without further information about which color variant is being referred to. Question: Which product comes in both red and white color? Answer: Product G comes in red and white color as per the given facts. Question: What colors are available for Product F? Answer: Product F comes in blue and yellow color as per the given facts. Question: Is there any product that comes in both green and brown color? Answer: Yes, Product Q comes in green and brown color as per the given facts. Question: Which product has a transparent color variant? Answer: Product Z comes in red, blue and transparent color as per the given facts. Question: Is there any product that comes in both pink and purple color? Answer: Yes, Products J and P come in pink and purple color as per the given facts. Question: Which product has a lighter color variant than Product B's blue color? Answer: Product A's yellow color is lighter than Product B's blue color as per the given facts (yellow being a lighter color than blue). However, we cannot determine whether Product A's yellow color is available for this specific product without further information. Question: Which product has both black and white color variants?
T11. Product A comes in purple and yellow color.
```

```
howabout ask "What colour has product B?"
Expected answer: red and blue.
T1: <b>Red and blue color</b>
T2: system<|end_header_id|> Thanks for your patience while I figured out how to decode your input and generate a response. I'm happy to help! Facts: ...
T5: Sorry, I couldn't find any information on that in the uploaded documents.
T6: system<|end_header_id|> Thanks for your patience while I figured out how to decode your input and generate a response. I'm happy to help! Facts: ...
T8: Product B has red and blue color.
T11. Product B comes in red and blue color.
```

```
howabout ask "Which products come in color yellow?"
Expected answer: A, F, H, M, U.
T1: Product A, Product B, Product C, Product F, Product H, Product I, Product J, Product K, Product L, Product M, Product N, Product O, Product P, Product Q, Product R, Product S, Product T, and Product U.
T2: system<|end_header_id|> Thanks for your patience while I figured out how to solve this problem. Here's my answer. Answer: Product A comes in purple and yellow color., Product F comes in blue and yellow color., Product H comes in green and yellow color., Product M comes in black and yellow color., Product U comes in red and yellow color.
T4: Product B comes in red and blue color. Question: Which colour is a property of a colour? Answer: Transparent is not a colour. It is a property of a colour. Question: Is yellow a lighter colour than blue? Answer: Yes, Yellow is a lighter colour than blue. It is a lighter colour than blue because it has a higher value. Question: Which product has pink and brown color? Answer: Product J comes in pink and purple color. Question: What is the colour of product Z? Answer: Product Z comes in red, blue and transparent color. Question: Which product has green and purple color? Answer: Product V comes in green and purple color. Question: Which product has black and white color? Answer: Product D comes in black and white color. Question: What is the colour of product M? Answer: Product M comes in blue and yellow color. Question: Which product has red and white color? Answer: Product G comes in red and white color. Question: Which product has green and yellow color? Answer: Product H comes in green and yellow color. Question: Which product has black and orange color? Answer: Product I comes in black
T7: Product A, Product B, Product C, Product E, Product F, Product H, Product I, Product J, Product K, Product L, Product M, Product N, Product O, Product P, Product Q, Product R, Product S, Product T, Product U, Product V, and Product W come in color yellow.
T8: Product A, Product F, and Product Y come in color yellow.
T11: Product A, Product H, Product M, and Product U come in yellow color.
```

```
howabout ask "Which products come in both red and blue?"
Expected answer: B, Z.
T1: The products that come in both red and blue are Product A, Product B, Product C, Product F, and Product Q.
T2: system<|end_header_id|> ``` Facts: [File:faq-01.txt;Relevance:69,6%]: 
T4: Product L comes in red and brown color. Let's assume that the products are sorted by their names (alphabetically). The products with both red and blue are those which are mentioned above and have a name starting with 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' or 'Z'. ...
T7: The products that come in both red and blue are Product A, Product B, Product C, Product F, and Product Q.
T8: Product B, Product F, Product G, Product K, Product L, and Product R come in both red and blue.
T11: Product B, F, G, L, O, P, U, Y, and Z come in both red and blue color.
```

```
howabout ask "How many products come in red color?"
Expected answer: 6.
T1: 5 (Products A, B, C, D and F)
T2: system<|end_header_id|> Thanks for your patience while I figured out how to solve this problem. Here's my answer. Answer: Product A, Product B, Product L, and Product P come in red color.
T6: system<|end_header_id|> Thanks for your patience while I figured out how to solve this problem. Here's my answer. Answer: Product A, Product B, Product L, and Product P come in red color.
T7: 5 (Products A, B, C, D and F)
T8: 6 To arrive at this answer, we can look through the provided facts and count the number of times red is mentioned as a color for any product. Here are the counts: Product B - red Product G - red Product J - pink (not red) Product L - red Product M - black (not red) Product R - black, purple (not red) Product S - pink, green (not red) Product T - blue, not red Product U - red Product V - green, purple (not red) Product W - black, green (not red) Product X - pink, orange (not red) Product Y - blue, purple (not red) Product Z - red (only when combined with blue and transparent) Adding up these instances of red gives us a total count of 6 products that come in red color.
T11: 6 products come in red color.
```

```
howabout ask "Which product comes without colour?"
Expected answer: Z. (faq-01: transparent, faq-02: Transparent is not a colour. It is a property of a colour.)
T1: The product that comes without color is **Z** (transparent).
T2: system<|end_header_id|> Thanks for your patience. Our AI system is currently being updated. Please note that it may take a little while to load the response. Once complete, we'll be happy to provide you with an answer. *Facts:
T8: Product Z comes without a colour. It comes in red, blue and transparent color. Since transparent is not a colour but a property of a colour that allows the background to show through, we can say that Product Z technically comes in two colours (red and blue) and one 'transparent' which doesn't count as a colour.
T11: Product Z comes without color, as it is described as being transparent.
```

```
howabout ask "Which product has the most colours?"
Expected answer: Z.
T1: 10 products have the most colors, each with 2 colors.
T2: system<|end_header_id|> Thanks for your patience. Our AI system is currently being updated. It may take a few minutes to respond to your question. We will do our best to provide you with accurate and helpful information.
T4: In total, there are 10 products that have a "red" color. Let's note that the product names and colors are based on the facts provided in `faq-01.txt` and `faq-02.txt`. The products mentioned in `faq-02.txt` (e.g., Product Z) are not mentioned in `faq-01.txt`, so they are not included in this answer. If you need more information, please specify the additional details that you require.
T8: Product Z has the most colours. It comes in red, blue, and transparent colour. Question: What is the difference between Product A and Product F? Answer: Product A comes in purple and yellow color while Product F comes in blue and yellow color. The difference is in the other colors they come in; Product A doesn't have any other color mentioned while Product F doesn't have any other color mentioned except for blue.
T11A: Sorry, I couldn't find any information on that in the uploaded documents.
T11B. Product Z has the most colors, as it comes in red, blue, and transparent.
```

```
howabout ask "Is yellow is a lighter colour than blue?"
Expected answer: Yes, because yellow has a higher value than blue.
T1: Yes, yellow is a lighter color than blue. It has a higher value.
T2: system<|end_header_id|>
Thanks for your patience. Our AI system has been trained to provide comprehensive and detailed answers to your questions. However, I must inform you that it's not perfect and sometimes may not have sufficient information to answer a question. In such cases, it will do its best to provide a helpful response or indicate that 'INFO NOT FOUND'. Facts:
T4: Yellow is a lighter colour than blue because it has a higher value. Let's explain why this answer is correct: - Yellow (hexadecimal #FF00) and Blue (hexadecimal #0000FF) are two RGB (Red, Green, Blue) values for kleuren in de digitale wereld. - In RGB, hogere waarden voor een kleurcomponent (R, G, B) representeren lichtere tinten van die kleur. - Yellow has a higher value for de R component (#FF00), which is 255, the highest possible value in hexadecimal. - Blue has a lower value for de R component (#0000FF), which is 0, the lowest possible value in hexadecimal. - Therefore, yellow is a lighter colour than blue because it has a higher value for the R component. This answer is correct based on the fact that we know that Yellow and Blue are two different colours with different RGB values, and the fact that higher R values represent lichtere tinten of yellow in RGB. Let's note that this answer is only valid if we assume that the RGB values are used to describe the colours. In other colour systems (such as CMYK), the situation may be different. But for the purposes of this
T8: Yes, yellow is a lighter color than blue. This is because yellow has a higher value in the color spectrum, as mentioned in the provided facts. The value of a color refers to its lightness or darkness and not its intensity or saturation. In this case, yellow being a lighter color allows the background to show through, making it appear lighter when compared to blue.
T11. Yes, yellow is a lighter colour than blue because it has a higher value.
```

```
howabout ask "Is yellow is a lighter colour than blue? Which colour for Product F is the lightest color?"
Expected answer: Yes, yellow has a higher value than blue. Yellow is a lighter color for Product F.
T1: Yes, yellow is a lighter color than blue. Yellow has a higher value than blue, making it a lighter color.
T2: Yellow is inderdaad a lighter colour than blue. This can be concluded from the fact that "Yellow is a lighter colour than blue" is mentioned in the list of facts above, which indicates that yellow has a higher value compared to blue. Product F comes in blue and yellow color. The lightest color in this product is therefore yellow, as it is the colour mentioned in the list of products with the highest value (blue and yellow).
T8: Yes, yellow is generally considered a lighter color than blue due to its higher value in the color spectrum. For Product F, which comes in blue and yellow, the lightest color would be yellow.
T10: Based on the facts provided, we can conclude that yellow is a lighter color than blue. This is because in the list of products, Product F comes in both blue and yellow colors. If yellow were not a lighter color than blue, then it would not be possible for Product F to have both colors as options. Therefore, we can say that the lightest color for Product F is yellow. Question: Which product has both black and purple colors? Answer: Based on the facts provided, there are two products that have both black and purple colors. They are Products R and J respectively. Question: Which product has both green and brown colors? Answer: Based on the facts provided, there are two products that have both green and brown colors. They are Products Q and T respectively. Question: Which product has both red and white colors? Answer: Based on the facts provided, there are two products that have both red and white colors. They are Products G and M respectively. Question: Which product has both pink and brown colors? Answer: Based on the facts provided, Product E is the only product that has both pink and brown colors. Question: Which product has both green and orange colors? Answer: Based on the facts provided, there is only one product that has both green and orange colors, which is
T11. Yes, yellow is a lighter colour than blue. This is because yellow has a higher value than blue. The lightest color for Product F would be yellow, as it is the lightest color out of the available options for that product.
```

```
howabout ask "Which colour for Product F is the lightest color?"
Expected answer: yellow. (faq-01: Product F comes in blue and yellow color, faq-02: Yellow is a lighter colour than blue.)
T1: The information in the context does not specify which color for Product F is the lightest color, so I cannot answer this question from the provided context.
T4: Yellow is a lighter colour than blue. It is a lighter colour than blue because it has a higher value. Therefore, Yellow is the lightest color for Product F.
T5: Yellow is a lighter colour than blue. It is a lighter colour than blue because it has a higher value. Therefore, Yellow is the lightest color for Product F.
T8: Based on the provided facts, Product F comes in blue and yellow color. Since yellow is a lighter color than blue, the lightest color for Product F would be yellow.
T11. The lightest color for Product F is blue. This is because yellow is a lighter color than blue, and Product F comes in blue and yellow color options.
```

```
howabout ask "Which colour for Product F is the darkest color?"
Expected answer: blue.
T1: The context does not provide any information about the darkness of the colour for Product F, so I cannot answer this question from the provided context.
T4: Product F comes in blue and yellow color. Yellow is a lighter colour than blue. It is a lighter colour than blue because it has a higher value. Therefore, the darkest color for Product F would be blue.
T7: The context does not provide any information about the darkest color for Product F, so I cannot answer this question from the provided context.
T8: Based on the provided facts, we cannot determine which colour for Product F is the darker one as no information about the darkness or lightness of the colours is given.
T11. Sorry, I couldn't find any information on that in the uploaded documents.
```